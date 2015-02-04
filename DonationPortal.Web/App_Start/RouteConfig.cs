using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DonationPortal.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Event Detail",
				url: "rider-detail/{eventUrlSlug}/{riderUrlSlug}",
				defaults: new { controller = "RiderDetail", action = "Index" }
			);

            routes.MapRoute(
                name: "Social",
                url: "social/{action}",
                defaults: new { controller = "Social", action = "Index" }
             );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
