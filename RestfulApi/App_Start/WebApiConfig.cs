using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RestfulApi
{
    /// <summary>
    /// 
    /// </summary>
	public static class WebApiConfig
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
		public static void Register(HttpConfiguration config)
		{
            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
