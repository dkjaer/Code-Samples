using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SharePoint
{
	/// <summary>
	/// 
	/// </summary>
    public class SharepointFile : SharepointObject
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
        public SharepointFile(string path)
        {
            base.path = CleanPath(path);
            var array = base.path.Split('/');
            for(int i = 0; i < array.Length - 1; i++)
            {
                parentPath += "/" + array[i];
            }

			parentPath = CleanPath(parentPath);
			name = array[array.Length - 1];
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parentFolder"></param>
		/// <param name="name"></param>
        public SharepointFile(SharepointFolder parentFolder, string name)
        {
            parentPath = parentFolder.Path;
			this.name = CleanPath(name);
			path = string.Format("{0}/{1}", parentPath, base.name);
        }
        
        /// <summary>
        /// Gets the time that this file was last modified, who modified it and the size of the file.
        /// </summary>
        /// <returns>
        /// This will return a SharePointFileResult structure.
        /// </returns>
        public SharePointFileResult GetInfo()
        {
			var result = new SharePointFileResult();
            errorMessage = string.Empty;
            try
            {
				var sourceUrl = string.Format("{0}{1}/{2}", Core.RootUrl, Encode(parentPath), Encode(name));
				var sourceUri = new Uri(sourceUrl);
                var file = SessionHelper.Client.Web.GetFileByServerRelativeUrl(sourceUri.AbsolutePath);
                SessionHelper.Client.Load(file);
                SessionHelper.Client.ExecuteQuery();
                result.ModifiedTime = file.TimeLastModified;
                result.SizeInBytes = Convert.ToInt32(file.Length);
                result.ModifiedBy = string.Empty;
			}
			catch (Exception exception)
			{
				Debug.WriteLine(exception.Message);
			}

			return result;
        }
        
        /// <summary>
        /// Checks if this file exists in the SharePoint storage.
        /// </summary>
        /// <returns>
        /// This will return true if this file exists.
        /// </returns>
        public bool Exists()
        {
			var result = false;
			errorMessage = string.Empty;
            try
            {
				var sourceUrl = string.Format("{0}{1}/{2}", Core.RootUrl, Encode(base.parentPath), Encode(base.name));
				var sourceUri = new Uri(sourceUrl);
                var file = SessionHelper.Client.Web.GetFileByServerRelativeUrl(sourceUri.AbsolutePath);
                SessionHelper.Client.Load(file);
                SessionHelper.Client.ExecuteQuery();
                result = (file != null && file.Exists);
            }
            catch(Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }

            return result;
        }

        /// <summary>
        /// Download a file from SharePoint.
        /// </summary>
        /// <param name="localFolder">
        /// This is the local folder that you want to download to.
        /// </param>
        /// <returns>
        /// This will return true unless an error occurs in which case, it will return a false.
        /// </returns>
        public bool Download(string localFolder)
        {
            bool result = true;
            errorMessage = string.Empty;
            try
            {
                var web = SessionHelper.Client.Web;
				var uri = new Uri(Core.RootUrl + Encode(path));
                var file = web.GetFileByServerRelativeUrl(uri.AbsolutePath);

				//// Create a stream.
				var data = file.OpenBinaryStream();
                SessionHelper.Client.Load(file);
                SessionHelper.Client.ExecuteQuery();

				//// Download the bytes into the new file.
				var destinationPath = CleanLocalPath(localFolder) + name;
                using(var destinationStream = System.IO.File.OpenWrite(destinationPath))
                {
                    try
                    {
						var buffer = new byte[8 * 1024];
                        int len;
                        while((len = data.Value.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            destinationStream.Write(buffer, 0, len);
                        }
                    }
                    catch (Exception exception)
                    {
                        errorMessage = exception.Message;
                        result = false;
                    }
                }
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                result = false;
            }

            return result;
        }

		/// <summary>
		/// Upload a file to the designated folder on SharePoint.
		/// </summary>
		/// <param name="localFilePath">
		/// This is the local folder that contains the file that you specify.
		/// </param>
		/// <param name="overwrite">
		/// If the file you specify already exists on SharePoint,
		/// it will be overwritten if this property is true.
		/// Otherwise the file specified will not be uploaded and 
		/// the existing file on SharePoint will remain intact.
		/// </param>
		/// <returns>
		/// This will return true unless an error occurs in which case, it will return a false.
		/// </returns>
		public bool Upload(string localFilePath, bool overwrite)
        {
			var result = false;
            errorMessage = string.Empty;
            try
            {
				var fileInfo = new FileInfo(localFilePath);
				if (overwrite || (!overwrite && !fileInfo.Exists))
				{
					using (var fs = new FileStream(localFilePath, FileMode.Open))
					{
						Microsoft.SharePoint.Client.File.SaveBinaryDirect(
							SessionHelper.Client,
							string.Format("/{0}/{1}/{2}", Core.DocumentRootName, Encode(base.parentPath), base.name),
							fs,
							true);
					}
				}

                result = true;
            }
            catch (PathTooLongException exception)
            {
                errorMessage = exception.Message;
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
            }

            return result;
        }
        
        /// <summary>
        /// Delete a file from SharePoint storage.
        /// </summary>
        /// <returns>
        /// This will return true unless an error occurs in which case, it will return a false.
        /// </returns>
        public bool Delete()
        {
            var result = true;
            errorMessage = string.Empty;
            try
            {
                var web = SessionHelper.Client.Web;
                var fileUri = new Uri(Core.RootUrl + Encode(path));
                Microsoft.SharePoint.Client.File file = web.GetFileByServerRelativeUrl(fileUri.AbsolutePath);
                file.DeleteObject();
                SessionHelper.Client.ExecuteQuery();
            }
            catch(Exception exception)
            {
                errorMessage = exception.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Copy a file from one SharePoint folder to another.
        /// </summary>
        /// <param name="destinationFolder">
        /// The folder that you intend to copy the file to.
        /// </param>
        /// <param name="overwrite">
        /// This indicates what to do if the destination file already exists.
        /// </param>
        /// <returns></returns>
        public bool Copy(string destinationFolder, bool overwrite)
        {
            var result = true;
            errorMessage = string.Empty;
            try
            {
                var web = SessionHelper.Client.Web;
                var sourceUrl = string.Format("{0}{1}/{2}", Core.RootUrl, Encode(parentPath), Encode(base.name));
				var sourceUri = new Uri(sourceUrl);
                var sourceFile = web.GetFileByServerRelativeUrl(sourceUri.AbsolutePath);

				var destinationUrl = Core.RootUrl + Encode(string.Format("{0}/{1}", CleanPath(destinationFolder), base.name));
				var destinationUri = new Uri(destinationUrl);
                var destinationFile = web.GetFileByServerRelativeUrl(destinationUri.AbsolutePath);

                bool destinationFound = false;
                try
                {
                    SessionHelper.Client.Load(destinationFile);
                    SessionHelper.Client.ExecuteQuery();
                    destinationFound = true;
                }
                catch(Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }

                if(!overwrite && destinationFound)
                {
                    //// This is the only scenario where we don't copy the file.
                }
                else
                {
                    sourceFile.CopyTo(destinationUrl, overwrite);
                    SessionHelper.Client.ExecuteQuery();
                }
            }
            catch(Exception exception)
            {
                errorMessage = exception.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Move a file from one SharePoint folder to another.
        /// This method can also be used to rename a file.
        /// </summary>
        /// <param name="destinationFolder">
        /// The folder that you intend to move the file to.
        /// </param>
        /// <param name="overwrite">
        /// This indicates what to do if the destination file already exists.
        /// </param>
        /// <returns></returns>
        public bool Move(string destinationFolder, bool overwrite)
        {
            bool result = true;
            errorMessage = string.Empty;
            try
            {
                var web = SessionHelper.Client.Web;

				var sourceUrl = Core.RootUrl + Encode(string.Format("{0}/{1}", parentPath, base.name));
				var sourceUri = new Uri(sourceUrl);
                var sourceFile = web.GetFileByServerRelativeUrl(sourceUri.AbsolutePath);

				var destinationUrl = Core.RootUrl + Encode(string.Format("{0}/{1}", CleanPath(destinationFolder), base.name));
				var destinationUri = new Uri(destinationUrl);
                var destinationFile = web.GetFileByServerRelativeUrl(destinationUri.AbsolutePath);

				var destinationFound = false;
                try
                {
                    SessionHelper.Client.Load(destinationFile);
                    SessionHelper.Client.ExecuteQuery();
                    destinationFound = true;
                }
                catch(Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }

                if(!overwrite && destinationFound)
                {
                    //// This is the only scenario where we don't move the file.
                }
                else
                {
                    sourceFile.CopyTo(destinationUrl, overwrite);
                    SessionHelper.Client.ExecuteQuery();

                    sourceFile.DeleteObject();
                    SessionHelper.Client.ExecuteQuery();

                    parentPath = destinationFolder;
                }
            }
            catch(Exception exception)
            {
                errorMessage = exception.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Open this SharePoint file in the default browser.
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            var result = true;
            errorMessage = string.Empty;
            var url = string.Empty;
            var s = name.Split(new char[] {'.'});
            var suffix = s[s.Length - 1];

            //// Create the list of suffixes that allow for
            //// collaboration in a browser.
            var suffixes = new List<string>();
            suffixes.Add("xlsx");
            suffixes.Add("docx");
            suffixes.Add("pptx");

            bool collaborateInBrowser = suffixes.Contains(suffix);
            if (collaborateInBrowser)
            {
                url = string.Format(
                    "{0}?sourcedoc=/{1}/{2}/{3}&action=edit",
                    Core.WopiUrl,
                    Encode(Core.DocumentRootName),
                    Encode(parentPath),
                    Encode(name));
            }
            else
            {
                url = string.Format(
                    "{0}/{1}/{2}/{3}",
                    Core.SharepointUrl,
                    Core.DocumentRootName,
                    Encode(parentPath),
                    Encode(name));
            }

            try
            {
                Process.Start(url);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                result = false;
            }

            return result;
        }
    }
}
