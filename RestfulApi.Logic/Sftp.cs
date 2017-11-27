using System;
using System.Diagnostics;
using System.IO;

namespace FileTransfer
{
    public class Sftp
    {
        Tamir.SharpSsh.Sftp sftp;
        string errorMessage = string.Empty;

        public event UpdateEventHandler TransferStart;
        protected virtual void OnTransferStart(UpdateEventArgs e)
        {
            TransferStart?.Invoke(this, e);
        }

        public event UpdateEventHandler TransferEnd;
        protected virtual void OnTransferEnd(UpdateEventArgs e)
        {
            TransferEnd?.Invoke(this, e);
        }

        public event UpdateEventHandler BinaryUpdate;
        protected virtual void OnBinaryUpdate(UpdateEventArgs e)
        {
            BinaryUpdate?.Invoke(this, e);
        }

        public string ErrorMessage
        {
            get
            {
                return errorMessage.Trim();
            }

            set
            {
                errorMessage = value.Trim();
            }
        }

        public Sftp()
        {
        }

        public Sftp(string url, string username, string password)
        {
            //settings = new FtpSettings();
            sftp = new Tamir.SharpSsh.Sftp(url, username, password);
            sftp.OnTransferProgress += Sftp_TransferProgress;
            sftp.OnTransferStart += Sftp_TransferStart;
            sftp.OnTransferEnd += Sftp_TransferEnd;
        }

        void Sftp_TransferStart(string source, string destination, int transferredBytes, int totalBytes, string message)
        {
            OnTransferStart(new UpdateEventArgs(transferredBytes, totalBytes));
        }

        void Sftp_TransferEnd(string source, string destination, int transferredBytes, int totalBytes, string message)
        {
            OnTransferEnd(new UpdateEventArgs(transferredBytes, totalBytes));
        }

        void Sftp_TransferProgress(string source, string destination, int transferredBytes, int totalBytes, string message)
        {
            OnBinaryUpdate(new UpdateEventArgs(transferredBytes, totalBytes));
        }

        public bool Connect()
        {
            var result = false;
            errorMessage = string.Empty;
            try
            {
                sftp.Connect();
                result = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return result;
        }

        public void Close()
        {
            try
            {
                sftp.Close();
            }
            catch (Exception exception)
            {
                Debug.Write(exception.Message);
            }
        }

        public bool Get(string fromFilePath, string toFilePath)
        {
            var result = false;
            errorMessage = string.Empty;
            try
            {
                var fileInfo = new FileInfo(toFilePath);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                sftp.Get(fromFilePath, toFilePath);
                result = true;
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
            }

            return result;
        }


        public bool Put(string filePath, string subFolder)
        {
            var result = false;
            try
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    sftp.Put(filePath, subFolder);
                }

                result = true;
            }
            catch (Exception exception)
            {
                Debug.Write(exception.Message);
            }

            return result;
        }

        public string[] Dir(string subFolder)
        {
            var result = new string[0];
            try
            {
                var files = sftp.GetFileList(subFolder);
                result = new string[files.Count];
                var i = 0;
                foreach (string file in files)
                {
                    result[i] = file;
                    i++;
                }
            }
            catch (Exception exception)
            {
                Debug.Write(exception.Message);
            }

            return result;
        }

        public void Quit()
        {
            sftp.Close();
        }
    }
}
