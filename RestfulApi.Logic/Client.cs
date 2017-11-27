using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace FileTransfer
{
    public class Client
    {
        public string ErrorMessage { get; set; }

        public event FileTransferEventHandler FileTransferBegin;
        protected virtual void OnFileTransferBegin(FileTransferEventArgs e)
        {
            FileTransferBegin?.Invoke(this, e);
        }

        public event FileTransferEventHandler ProgressChanged;
        protected virtual void OnProgressChanged(FileTransferEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }

        public event FileTransferEventHandler StatusChanged;
        protected virtual void OnStatusChanged(FileTransferEventArgs e)
        {
            StatusChanged?.Invoke(this, e);
        }

        public event FileTransferEventHandler FileTransferDone;
        protected virtual void OnFileTransferDone(FileTransferEventArgs e)
        {
            FileTransferDone?.Invoke(this, e);
        }

        public event FileTransferEventHandler ErrorOccurred;
        protected virtual void OnErrorOccurred(FileTransferEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        public bool Upload(FtpSettings settings, string localFilePath)
        {
            bool result = false;
            var file = new FileInfo(localFilePath);
            if (!file.Exists)
            {
                ErrorMessage = "Unable to access SharePoint file.";
            }
            else
            {
                // This will allow all certificates to be accepted.
                ServicePointManager.ServerCertificateValidationCallback += delegate (
                    object sender,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                if (settings.UseSsh)
                {
                    var sftp = new Sftp(settings.Url, settings.Username, settings.Password);
                    sftp.BinaryUpdate += BinaryUpdate;
                    OnStatusChanged(new FileTransferEventArgs("Logging in to FTP server."));
                    var loginSuccessful = sftp.Connect();
                    if (!loginSuccessful)
                    {
                        ErrorMessage = "Unable to login to FTP host.";
                    }
                    else
                    {
                        OnStatusChanged(new FileTransferEventArgs("Uploading " + file.Name));
                        sftp.Put(file.FullName, settings.SubFolder);

                        // Now, check the server to make sure the file is there...
                        var fileList = sftp.Dir(settings.SubFolder);
                        var found = false;
                        if (settings.CheckForFile)
                        {
                            foreach (var fileName in fileList)
                            {
                                if (fileName == file.Name)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            found = true;
                        }

                        sftp.Quit();

                        // Indicate whether or not the file is on the FTP server...
                        if (found)
                        {
                            result = true;
                            OnStatusChanged(new FileTransferEventArgs("Upload was successful!!  Logging out."));
                        }
                        else
                        {
                            ErrorMessage = "FTP was available but the upload failed!!";
                            OnStatusChanged(new FileTransferEventArgs(ErrorMessage));
                        }
                    }
                }
                else
                {
                    try
                    {
                        var fileStream = new FileStream(file.FullName, FileMode.Open);
                        try
                        {
                            var ftp = new Ftp(settings.Url) { ConnectMode = settings.ConnectMode };
                            ftp.BinaryUpdate += BinaryUpdate;
                            OnStatusChanged(new FileTransferEventArgs("Logging in to FTP server."));
                            var loginSuccessful = false;
                            try
                            {
                                ftp.Login(settings.Username, settings.Password);
                                loginSuccessful = true;
                            }
                            catch (Exception exception)
                            {
                                Debug.Write(exception.Message);
                            }

                            if (!loginSuccessful)
                            {
                                ErrorMessage = "Unable to login to FTP host.";
                            }
                            else
                            {
                                OnStatusChanged(new FileTransferEventArgs(string.Format("Changing to the '{0}' folder.", settings.SubFolder)));
                                if (settings.SubFolder != string.Empty)
                                {
                                    ftp.Chdir(settings.SubFolder);
                                }

                                // If the file was previously uploaded, delete the old copy.
                                if (settings.CheckForFile)
                                {
                                    try
                                    {
                                        ftp.Delete(file.Name);
                                    }
                                    catch (Exception exception)
                                    {
                                        Console.WriteLine(exception.Message);
                                    }
                                }

                                // Upload the file...
                                OnStatusChanged(new FileTransferEventArgs("Uploading " + file.Name));
                                ftp.TransferType = settings.TransferType;
                                ftp.Put(fileStream, file.Name);

                                // Now, check the server to make sure the file is there...
                                var found = false;
                                if (settings.CheckForFile)
                                {
                                    var fileNames = ftp.Dir();
                                    foreach (var fileName in fileNames)
                                    {
                                        if (fileName.ToLower() == file.Name.ToLower())
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    found = true;
                                }

                                ftp.Quit();

                                // Indicate whether or not the file is on the FTP server...
                                if (found)
                                {
                                    result = true;
                                    OnStatusChanged(new FileTransferEventArgs("Upload was successful!!  Logging out."));
                                }
                                else
                                {
                                    ErrorMessage = "FTP was available but the upload failed!!";
                                    OnErrorOccurred(new FileTransferEventArgs(ErrorMessage));
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorMessage = exception.ToString();
                            OnErrorOccurred(new FileTransferEventArgs(ErrorMessage));
                        }

                        fileStream.Close();
                    }
                    catch (Exception exception)
                    {
                        ErrorMessage = exception.ToString();
                        OnErrorOccurred(new FileTransferEventArgs(ErrorMessage));
                    }
                }

                file.Delete();
            }

            return result;
        }

        public List<string> FolderList(FtpSettings settings)
        {
            var result = new List<string>();
            var error = string.Empty;

            // This will allow all certificates to be accepted.
            ServicePointManager.ServerCertificateValidationCallback += delegate (
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            if (settings.UseSsh)
            {
                var sftp = new Sftp(settings.Url, settings.Username, settings.Password);
                OnStatusChanged(new FileTransferEventArgs("Logging in to FTP server."));
                var loginSuccessful = sftp.Connect();
                if (!loginSuccessful)
                {
                    ErrorMessage = "Unable to login to FTP host.";
                }
                else
                {
                    string[] list = sftp.Dir(settings.SubFolder);
                    foreach (var item in list)
                    {
                        result.Add(item);
                    }

                    sftp.Quit();
                }
            }
            else
            {
                try
                {
                    var ftp = new Ftp(settings.Url) { ConnectMode = settings.ConnectMode };
                    OnStatusChanged(new FileTransferEventArgs("Logging in to FTP server."));
                    try
                    {
                        var loginSuccessful = false;
                        try
                        {
                            ftp.Login(settings.Username, settings.Password);
                            loginSuccessful = true;
                        }
                        catch (Exception exception)
                        {
                            Debug.Write(exception.Message);
                        }

                        if (!loginSuccessful)
                        {
                            ErrorMessage = "Unable to login to FTP host.";
                        }
                        else
                        {

                            try
                            {
                                string[] list = ftp.Dir(settings.SubFolder);
                                foreach (var item in list)
                                {
                                    result.Add(item);
                                }
                            }
                            catch (Exception exception)
                            {
                                ErrorMessage = exception.Message;
                            }

                            ftp.Quit();
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorMessage = exception.Message;
                    }
                }
                catch (Exception exception)
                {
                    ErrorMessage = exception.ToString();
                    OnErrorOccurred(new FileTransferEventArgs(ErrorMessage));
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BinaryUpdate(object sender, UpdateEventArgs e)
        {
            OnProgressChanged(new FileTransferEventArgs(e.Current, e.Total));
        }
    }
}
