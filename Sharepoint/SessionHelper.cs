using System;
using System.Diagnostics;
using Microsoft.SharePoint.Client;

namespace SharePoint
{
    /// <summary>
    /// This is the central object used for performing SharePoint file and folder functions.
    /// </summary>
    public class SessionHelper
    {
        static ClientContext client = new ClientContext(Core.SharepointUrl);
                
        /// <summary>
        /// This is the actual live SharePoint session.
        /// </summary>
        /// <value>
        /// This needs to be passed to various SharePoint objects.
        /// </value>
        public static ClientContext Client
        {
            get
            {
                return client;
            }

            set
            {
                client = value;
            }
        }
				
        /// <summary>
        /// Member variables.
        /// </summary>
        string errorMessage = string.Empty;
				
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
        /// This is the method used to login to SharePoint and create a session.
        /// </summary>
        /// <param name="sharepointUsername">
        /// This is the user's login name for their SharePoint account. 
        /// This will look like "something@asallc.com".
        /// </param>
        /// <param name="sharepointPassword">
        /// This is the user's password for their SharePoint account.
        /// </param>
        /// <returns>
        /// This returns true if the login was successful.
        /// </returns>
        public bool Login(string sharepointUsername, string sharepointPassword)
        {
            var result = false;
            errorMessage = string.Empty;
            try
            {
                client = new ClientContext(Core.SharepointUrl);
                result = Security.Login(client, sharepointUsername, sharepointPassword);
                if(result)
                {
                    Sharepoint.Registry.SetKey(Sharepoint.Registry.SharepointUserKey, sharepointUsername);
                }
                else
                {
                    errorMessage = "Unsuccessful Logon";
                }
            }
            catch(Exception exception)
            {
                errorMessage = exception.Message;
            }

            return result;
        }
		
        /// <summary>
        /// This is the method used to login to SharePoint and create a session.
        /// </summary>
        /// <param name="sharepointUsername">
        /// This is the user's login name for their SharePoint account. 
        /// This will look like "something@asallc.com".
        /// </param>
        /// <param name="sharepointPassword">
        /// This is the user's password for their SharePoint account.
        /// </param>
        /// <returns>
        /// This returns true if the login was successful.
        /// </returns>
        public bool LoginUsingService(string sharepointUsername, string sharepointPassword)
        {
            var result = false;
            errorMessage = string.Empty;
            try
            {
                client = new ClientContext(Core.SharepointUrl);
                result = Security.LoginUsingService(client, sharepointUsername, sharepointPassword);
                if(result)
                {
                    Sharepoint.Registry.SetKey(Sharepoint.Registry.SharepointUserKey, sharepointUsername);
                }
                else
                {
                    errorMessage = "Unsuccessful Logon";
                }
            }
            catch(Exception exception)
            {
                errorMessage = exception.Message;
            }

            return result;
        }
		
        public bool Test()
        {
            var result = false;
            errorMessage = string.Empty;
            try
            {
                ClientContext c = client;
                //result = Security.LoginUsingService(client, sharepointUsername, sharepointPassword);
                //if (result)
                //{
                //    Registry.SetKey(Registry.SharepointUserKey, sharepointUsername);
                //}
                //else
                //{
                //    errorMessage = "Unsuccessful Logon";
                //}
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
            }

            return result;
        }
		
        /// <summary>
        /// This closes the SharePoint session.
        /// </summary>
        public void Close()
        {
            if(client != null)
            {
                client.Dispose();
            }

            client = null;
        }
		
        /// <summary>
        /// Check if the Sharepoint connection is still valid.
        /// </summary>
        public bool IsConnected()
        {
            var result = false;
            try
            {
                var site = client.Web;
                client.Load(site);
                client.ExecuteQuery();
                result = true;
            }
            catch (Exception exception)
            {
				Debug.WriteLine(exception.Message);
            }

            return result;
        }
		
        /// <summary>
        /// Get the last valid username.
        /// </summary>
        public static string GetPreviousUsername()
        {
            return Sharepoint.Registry.GetKey(Sharepoint.Registry.SharepointUserKey);
        }		
    }
}
