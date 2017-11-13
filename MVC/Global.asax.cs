using System;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.IdentityModel.Services;

namespace Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class MvcApplication : System.Web.HttpApplication
	{
		/// <summary>
		/// 
		/// </summary>
		protected void Application_Start()
		{
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_Error(object sender, EventArgs e)
		{
			// Be sure to reference System.IdentityModel.Services
			// and include using System.IdentityModel.Services; 
			// at the start of your class

			var error = Server.GetLastError();
			var cryptographicException = error as CryptographicException;
			if (cryptographicException != null)
			{
				FederatedAuthentication.WSFederationAuthenticationModule.SignOut();
				Server.ClearError();
			}
		}

		//protected void Application_BeginRequest()
		//{
		//    if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
		//    {
		//        Response.Flush();
		//    }
		//}
	}
}
