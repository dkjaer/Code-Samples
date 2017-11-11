using System;
using System.Diagnostics;
using System.Net;
using System.Security;
using Microsoft.SharePoint.Client;

namespace SharePoint
{
    public static class Security
    {
        /// <summary>
        /// Login using the login screen.
        /// </summary>
        /// <param name="clientContext"></param>
        /// <param name="sharepointUsername"></param>
        /// <param name="sharepointPassword"></param>
        /// <returns></returns>
        public static bool Login(ClientContext clientContext, string sharepointUsername, string sharepointPassword)
        {
            var result = false;
            try
            {
                var secureString = new SecureString();
                foreach(char c in sharepointPassword.ToCharArray())
                {
                    secureString.AppendChar(c);
                }
                
                clientContext.Credentials = new SharePointOnlineCredentials(sharepointUsername, secureString);
				var web = clientContext.Web;
                clientContext.Load(web);
                clientContext.ExecuteQuery();
                result = true;
            }
            catch(Exception exception)
            {
				var message = exception.Message;
            }

            return result;
        }

        /// <summary>
        /// Login using the login service.
        /// </summary>
        /// <param name="clientContext"></param>
        /// <param name="sharepointUsername"></param>
        /// <param name="sharepointPassword"></param>
        /// <returns></returns>
        public static bool LoginUsingService(ClientContext clientContext, string sharepointUsername, string sharepointPassword)
        {
            var result = false;
            try
            {
                var secureString = new SecureString();
                foreach(var c in sharepointPassword.ToCharArray())
                {
                    secureString.AppendChar(c);
                }
                
                var onlineCredentials = new SharePointOnlineCredentials(sharepointUsername, secureString);
                var ups = new UserProfileService();
                ups.UseDefaultCredentials = false;
                ups.Credentials = onlineCredentials;

				var targetSite = new Uri(Core.SharepointUrl);
				var authCookieValue = onlineCredentials.GetAuthenticationCookie(targetSite);
                ups.CookieContainer = new CookieContainer();
                ups.CookieContainer.Add(new Cookie(
                    "FedAuth",
                    authCookieValue.TrimStart("SPOIDCRL=".ToCharArray()), // Remove the prefix from the cookie's value
                    string.Empty,
                    targetSite.Authority));

                var claimsLogin = "i:0#.f|membership|" + sharepointUsername;
                var accName = ups.GetUserPropertyByAccountName(claimsLogin, "AccountName");
                result = true;
            }
            catch(Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }

            return result;
        }
    }
}
