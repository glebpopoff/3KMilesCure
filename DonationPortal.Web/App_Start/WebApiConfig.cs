using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DonationPortal.Web.App_Start;
using DonationPortal.Web.Attributes;

namespace DonationPortal.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			// Web API configuration and services
			config.Filters.Add(new ExceptionResponseAttribute());

            // Web API routes
			config.MapHttpAttributeRoutes();

			// respond w/ JSON when browsers ask for HTML.
			config.Formatters.Add(new BrowserJsonFormatter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
