using System.Web.Http;
using System.Web.Http.Cors;

namespace Mvc
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			var cors = new EnableCorsAttribute("*", "*", "*");
			config.EnableCors(cors);
			//config.MessageHandlers.Add(new Helpers.OptionsHttpMessageHandler());
			config.MapHttpAttributeRoutes();

			//config.Routes.MapHttpRoute(
			//	name: "DefaultApi",
			//	routeTemplate: "api/{controller}/{action}/{id}",
			//	defaults: new { id = RouteParameter.Optional }
			//);

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
