using System;
using System.Net;
using Microsoft.SharePoint.Client;

namespace SharePoint
{
    internal class UserProfileService
    {
        public UserProfileService()
        {
        }

        public CookieContainer CookieContainer { get; internal set; }
        public bool UseDefaultCredentials { get; internal set; }
        public SharePointOnlineCredentials Credentials { get; internal set; }

        internal object GetUserPropertyByAccountName(string claimsLogin, string v)
        {
            throw new NotImplementedException();
        }
    }
}