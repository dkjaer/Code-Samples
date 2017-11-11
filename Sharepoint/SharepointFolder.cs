using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.SharePoint.Client;

namespace SharePoint
{
	/// <summary>
	/// 
	/// </summary>
    public class SharepointFolder : SharepointObject
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
        public SharepointFolder(string path)
        {
            base.path = CleanPath(path);
            var array = base.path.Split('/');            
            for(var i = 0; i < array.Length - 1; i++)
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
        public SharepointFolder(SharepointFolder parentFolder, string name)
        {
            parentPath = parentFolder.Path;
            base.name = CleanPath(name);
            path = string.Format("{0}/{1}", parentPath, base.name);
        }
		
        /// <summary>
        /// Get a list of sub folders and files in a SharePoint directory.
        /// </summary>
        /// <returns>
        /// This will return all of the folders and files in the form of SharePoinObject objects.
        /// The folders will be type SharepointObjectType.Folder.
        /// The files will be type SharepointObjectType.File.
        /// </returns>
        public List<SharepointObject> GetContents()
        {
            var result = new List<SharepointObject>();
            errorMessage = string.Empty;
            try
            {
                var url = Core.RootUrl + Encode(path);

                //// Add the folders.
                var folders = SessionHelper.Client.Web.GetFolderByServerRelativeUrl(url).Folders;
                SessionHelper.Client.Load(folders);
                SessionHelper.Client.ExecuteQuery();
                foreach(var folder in folders)
                {
                    result.Add(new SharepointFolder(
                        folder.ServerRelativeUrl.Replace(Core.DocumentRootName + "/", string.Empty)));
                }

                //// Add the files.
                var files = SessionHelper.Client.Web.GetFolderByServerRelativeUrl(url).Files;
                SessionHelper.Client.Load(files);
                SessionHelper.Client.ExecuteQuery();
                foreach(var file in files)
                {
                    result.Add(new SharepointFile(path + "/" + file.Name));
                }
            }
            catch(Exception exception)
            {
                errorMessage = exception.Message;
            }

            return result;
        }
		
        /// <summary>
        /// Get a list of sub folders in a SharePoint directory.
        /// </summary>
        /// <returns>
        /// This will return all of the folders in the form of SharepointFolder objects.
        /// </returns>
        public List<SharepointFolder> GetFolders()
        {
            var result = new List<SharepointFolder>();
            errorMessage = string.Empty;
            try
            {
                var url = Core.RootUrl + Encode(path);

                //// Add the folders.
                var folders = SessionHelper.Client.Web.GetFolderByServerRelativeUrl(url).Folders;
                SessionHelper.Client.Load(folders);
                SessionHelper.Client.ExecuteQuery();
                foreach (var folder in folders)
                {
                    result.Add(new SharepointFolder(
                        folder.ServerRelativeUrl.Replace(Core.DocumentRootName + "/", string.Empty)));
                }
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
            }

            return result;
        }
		
        /// <summary>
        /// Get a list of files in a SharePoint directory.
        /// </summary>
        /// <returns>
        /// This will return all of the files in the form of SharepointFile objects.
        /// </returns>
        public List<SharepointFile> GetFiles(string pattern)
        {
            var result = new List<SharepointFile>();
            errorMessage = string.Empty;
            try
            {
				var encoded = Encode(path.Replace(@"\", "/"));
				var url = Core.RootUrl + encoded;
                var newPattern = pattern.ToLower().Replace("*", "(.*?)");
                var regex = new Regex(newPattern, RegexOptions.IgnoreCase);

				//// Add the files.
				var files = SessionHelper.Client.Web.GetFolderByServerRelativeUrl(url).Files;
				SessionHelper.Client.Load(files);
				SessionHelper.Client.ExecuteQuery();
				var fileList = files.ToList();
				var fileMatches = fileList.Where(x => regex.IsMatch(x.Name));
				foreach (var file in fileMatches)
				{
					result.Add(new SharepointFile(string.Format("{0}/{1}", path, file.Name)));
				}
			}
            catch (Exception exception)
            {
                errorMessage = exception.Message;
            }

            return result;
        }
		
        /// <summary>
        /// Checks if this folder exists in the SharePoint storage.
        /// </summary>
        /// <returns>
        /// This will return true if this folder exists.
        /// </returns>
        public bool Exists()
        {
            var result = false;
            errorMessage = string.Empty;
            try
            {
                var web = SessionHelper.Client.Web;
                var folderUrl = Core.RootUrl + Encode(path);
                var folder = web.GetFolderByServerRelativeUrl(folderUrl);
                SessionHelper.Client.Load(folder);
                SessionHelper.Client.ExecuteQuery();
                result = (folder != null);
            }
            catch(Exception exception)
            {
                errorMessage = exception.Message;
            }

            return result;
        }
		
        /// <summary>
        /// Creates a new folder in the SharePoint storage.
        /// </summary>
        /// <returns>
        /// This will return true unless an error occurs in which case, it will return a false.
        /// </returns>
        public bool Create()
        {
            var result = false;
            errorMessage = string.Empty;
            try
            {
                var web = SessionHelper.Client.Web;
                var folderUrl = Core.RootUrl + Encode(base.parentPath);
                var folders = web.GetFolderByServerRelativeUrl(folderUrl).Folders;
                SessionHelper.Client.Load(folders);
                SessionHelper.Client.ExecuteQuery();

                //// Check if the folder already exists.
                
                foreach(var subFolder in folders)
                {
                    if(subFolder.Name == name)
                    {
                        result = true;
                        break;
                    }
                }

                //// If not then create a new one.
                if(!result)
                {
                    folders.Add(name);
                    SessionHelper.Client.ExecuteQuery();
                }
            }
            catch(Exception exception)
            {
                errorMessage = exception.Message;
            }

            return result;
        }
		
        /// <summary>
        /// Deletes an entire SharePoint folder AND its contents.
        /// </summary>
        /// <returns>
        /// This will return true unless an error occurs in which case, it will return a false.
        /// </returns>
        public bool Delete()
        {
            bool result = false;
            errorMessage = string.Empty;
            try
            {
                var web = SessionHelper.Client.Web;
                var folderUrl = Core.RootUrl + Encode(base.parentPath);
                var folders = web.GetFolderByServerRelativeUrl(folderUrl).Folders;
                SessionHelper.Client.Load(folders);
                SessionHelper.Client.ExecuteQuery();

                //// Check if the folder already exists.
                foreach(var subFolder in folders)
                {
                    if(subFolder.Name == name)
                    {
                        subFolder.DeleteObject();
                        SessionHelper.Client.ExecuteQuery();
                        result = true;
                        break;
                    }
                }
            }
            catch(Exception exception)
            {
                errorMessage = exception.Message;
            }

            return result;
        }
    }
}
