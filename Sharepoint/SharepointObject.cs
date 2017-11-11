using System;
using System.Diagnostics;
using Microsoft.SharePoint.Client;

namespace SharePoint
{
    public class SharepointObject
    {
        /// <summary>
        /// Member variables.
        /// </summary>
        protected string errorMessage = string.Empty;
        protected string path = string.Empty;
        protected string name = string.Empty;
        protected string parentPath = string.Empty;

        /// <summary>
        /// This is the name of the object.
        /// </summary>
        public string Path
        {
            get
            {
                return path.Trim();
            }

            set
            {
                path = CleanPath(value);
            }
        }

        /// <summary>
        /// This is the name of the object.
        /// </summary>
        public string Name
        {
            get
            {
                return name.Trim();
            }

            set
            {
                name = CleanPath(value);
            }
        }

        /// <summary>
        /// This is the path of the of the parent folder.
        /// </summary>
        public string ParentPath
        {
            get
            {
                return parentPath.Trim();
            }

            set
            {
                parentPath = CleanPath(value);
            }
        }

        /// <summary>
        /// Read-only error message property.
        /// </summary>
        /// <value>
        /// This will be an empty string unless an error occurred.
        /// If an error occurred, then this will contain a message with details about it.
        /// </value>
        public string ErrorMessage
        {
            get
            {
                return errorMessage.Trim();
            }
        }
		
        /// <summary>
        /// This is used to trim a path string and strip off any ending slashes.
        /// </summary>
        /// <param name="input">
        /// This is the original path name that you intend to 'clean'.
        /// </param>
        /// <returns>
        /// This is the resulting 'clean' path name.
        /// </returns>
        protected string CleanPath(string input)
        {
            var result = input.Trim();
            try
            {
                while(result.StartsWith("/", StringComparison.Ordinal))
                {
                    result = result.Substring(1);
                }
			}
			catch (Exception exception)
			{
				Debug.WriteLine(exception.Message);
			}

			try
            {
                while(result.EndsWith("/", StringComparison.Ordinal))
                {
                    result = result.Substring(0, result.Length - 1);
                }
            }
            catch(Exception exception)
			{
				Debug.WriteLine(exception.Message);
			}

            return result;
        }

        /// <summary>
        /// This is used to trim a path string and strip off any ending slashes.
        /// </summary>
        /// <param name="input">
        /// This is the original path name that you intend to 'clean'.
        /// </param>
        /// <returns>
        /// This is the resulting 'clean' path name.
        /// </returns>
        protected string CleanLocalPath(string input)
        {
            var result = input.Trim();
            if(!result.EndsWith(@"\", StringComparison.Ordinal))
            {
                result += @"\";
            }

            return result;
        }

        /// <summary>
        /// This converts the input into a string encoded for using in a url.
        /// </summary>
        /// <param name="input">
        /// This can be any string.
        /// </param>
        /// <returns>
        /// This returns a url encoded string.
        /// </returns>
        public string Encode(string input)
        {
            //// Analytics/Muni Opportunities 2010
            //// Analytics/Muni%20Opportunities%202010
            var result = input.Replace("/", "QQQQQ");
            result = result.Replace(" ", "SPSPSPSPSP");
            result = System.Web.HttpUtility.UrlEncode(result);
            result = result.Replace("QQQQQ", "/");
            result = result.Replace("SPSPSPSPSP", "%20");
            return result.Trim();
        }
    }
}
