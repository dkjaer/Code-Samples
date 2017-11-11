using System;
using System.Web;

namespace SharePoint
{
    public static class Core
    {        
        /// <summary>
        /// The base SharePoint url.
        /// </summary>
        /// <remarks> 
        /// This should be converted to a property eventually.
        /// </remarks>
        public const string SharepointUrl = "https://asaent.sharepoint.com";

        /// <summary>
        /// The name of the root folder.
        /// </summary>
        public const string DocumentRootName = "Shared Documents";

        /// <summary>
        /// The URL of the root folder for the document storage.
        /// </summary>
        public const string RootUrl = SharepointUrl + "/" + DocumentRootName + "/";

        /// <summary>
        /// Web Application Open Platform Interface Protocol
        /// </summary>
        public const string WopiUrl = SharepointUrl + "/_layouts/15/WopiFrame.aspx";

        /// <summary>
        /// This is a local temporary folder used for placing files that need to be modified and re-uploaded.
        /// </summary>
        /// <remarks> 
        /// Server 2012 doesn't allow access to various folders including "C:\".  After 
        /// trying other methods and locations such as...
        ///
        /// > System.Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Process);
        /// > "C:\WebTemp\";
        ///
        /// ...I searched the net and found the folder below can be accessed by the web server
        /// </remarks>
        public const string TempFolderPath = @"C:\Windows\SysWOW64\config\systemprofile\Desktop\";
		
        /// <summary>
        /// Get the number of subfolders in the root of the SharePoint directory.
        /// </summary>
        /// <returns>
        /// This will return the number of the folders in the root of the SharePoint directory.
        /// </returns>
        public static void SetMaxFileUploadSize()
        {
            try
            {
                //// Query the folders.
                
                //var regionalSettings = SessionHelper.Client.Site.
                //SessionHelper.Client.Load(regionalSettings);
                //SessionHelper.Client.Load(regionalSettings.TimeZone);
                //SessionHelper.Client.ExecuteQuery();
            }
            catch(Exception exception)
            {
                string error = exception.Message;
            }
        }
		
        /// <summary>
        /// Get the number of subfolders in the root of the SharePoint directory.
        /// </summary>
        /// <returns>
        /// This will return the number of the folders in the root of the SharePoint directory.
        /// </returns>
        public static int GetRootFolderCount()
        {
            var result = 0;
            try
            {
                //// Query the folders.
                var folders = SessionHelper.Client.Web.GetFolderByServerRelativeUrl(Core.RootUrl).Folders;
                SessionHelper.Client.Load(folders);
                SessionHelper.Client.ExecuteQuery();
                result = folders.Count;
            }
            catch(Exception exception)
            {
                string error = exception.Message;
            }

            return result;
        }
				
        /// <summary>
        /// Get the number of subfolders in the root of the SharePoint directory.
        /// </summary>
        /// <returns>
        /// This will return the number of the folders in the root of the SharePoint directory.
        /// </returns>
        public static int GetTimeZoneFactor()
        {
            var result = 0;
            try
            {
                //// Query the folders.
                var regionalSettings = SessionHelper.Client.Web.RegionalSettings;
                SessionHelper.Client.Load(regionalSettings);
                SessionHelper.Client.Load(regionalSettings.TimeZone);
                SessionHelper.Client.ExecuteQuery();

                var timezone = regionalSettings.TimeZone;
                result = -timezone.Information.Bias / 60;
            }
            catch(Exception exception)
            {
                var error = exception.Message;
            }

            return result;
		}

        /// <summary>
        /// Returns the name of a folder using the number of folders represented
        /// in the HTTP request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pathDepth"></param>
        /// <returns></returns>
        public static string GetSharePointFolder(HttpRequest request, int pathDepth = 2)
		{
            var result = string.Empty;
            var i = request.ServerVariables["PATH_TRANSLATED"].LastIndexOf('\\');
            var s = request.ServerVariables["PATH_TRANSLATED"].Substring(0, i);
			if (s.EndsWith("\\", StringComparison.Ordinal))
			{
				s = s.Substring(0, s.Length - 1);
			}

			i = s.LastIndexOf('\\');
			for (int j = 1; j <= pathDepth; j++)
			{
				i = s.Substring(0, i).LastIndexOf('\\');
			}

			result = s.Substring(0, i) + "\\SharePoint Sync";
			return result;
		}

        /// <summary>
        /// Returns the name of a folder using the number of folders represented
        /// in the HTTP request (base class).
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pathDepth"></param>
        /// <returns></returns>
        public static string GetSharePointFolder(HttpRequestBase request, int pathDepth = 2)
		{
            var result = string.Empty;
            var i = request.ServerVariables["PATH_TRANSLATED"].LastIndexOf('\\');
            var s = request.ServerVariables["PATH_TRANSLATED"].Substring(0, i);
			if (s.EndsWith("\\", StringComparison.Ordinal))
			{
				s = s.Substring(0, s.Length - 1);
			}

			i = s.LastIndexOf('\\');
			for (int j = 1; j <= pathDepth; j++)
			{
				i = s.Substring(0, i).LastIndexOf('\\');
			}

			result = s.Substring(0, i) + "\\SharePoint Sync";
			return result;
		}
	}
}
