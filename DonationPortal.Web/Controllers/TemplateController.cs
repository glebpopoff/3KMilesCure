using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationPortal.Engine;
using DonationPortal.Web.ViewModels.Home;
using DonationPortal.Web.ViewModels.Template;
using System.Data.Entity;

namespace DonationPortal.Web.Controllers
{
	[ChildActionOnly]
    public class TemplateController : Controller
    {
        public ActionResult Header()
        {
	        var urlHelper = new UrlHelper(this.ControllerContext.RequestContext);

	        using (var entities = new DonationPortalEntities())
	        {
				var model = new HeaderViewModel
				{
					// another symptom of having 1:1 between riders and events at the moment...
					Riders = entities.EventRiders.Include(r => r.Event).ToList().Select(rider => new EventViewModel
					{
						Name = rider.Event.Name,
						Url = urlHelper.Action("Index", "RiderDetail", new { EventUrlSlug = rider.Event.UrlSlug, RiderUrlSlug = rider.UrlSlug }),
					}),
					FeaturedEventTitle = entities.Events.First().Name // todo, have a way to choose the featured event.
				};
            
				return View("Header", model);
	        }

        }

		public ActionResult Footer()
		{
			return View();
		}
    }
}