using System.Configuration;
using System.Web.Http.Controllers;
using System.Linq;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.SharePoint.Client;

namespace SharePoint
{
	public class Helper
    {
        static AuthenticationResult AcquireUserAssertionAccessToken(
            HttpControllerContext controllerContext,
            AuthenticationContext authContext,
            string resource)
        {
            var token = controllerContext.Request.Headers.GetValues("Authorization").First();
            var userAccessToken = token.Substring(token.LastIndexOf(' ')).Trim(); //To remove the "Bearer"
            var clientId = ConfigurationManager.AppSettings["ida:ClientId"];
            var clientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
            var clientCredential = new ClientCredential(clientId, clientSecret);
            var userAssertion = new UserAssertion(userAccessToken);
            var result = authContext.AcquireToken(resource, clientCredential, userAssertion);
            return result;
        }

        public static ClientContext GetClientContextWithAccessToken(
			HttpControllerContext controllerContext)
		{
			var result = new ClientContext(Core.RootUrl);
            var aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
            var tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
            var authority = string.Format("{0}{1}", aadInstance, tenantId);
            var audience = ConfigurationManager.AppSettings["ida:Audience"];
            var resource = audience;
            var authContext = new AuthenticationContext(authority);
			var authResult = AcquireUserAssertionAccessToken(controllerContext, authContext, resource);
			return result;
		}
	}
}