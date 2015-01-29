using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationPortal.Engine;
using DonationPortal.Web.ViewModels.RiderDetail;

namespace DonationPortal.Web.Controllers
{
    public class RiderDetailController : Controller
    {
        // GET: EventDetail
        public ActionResult Index(string eventUrlSlug, string riderUrlSlug)
        {
	        using (var entities = new DonationPortalEntities())
	        {
				var eventEntity = entities.Events.SingleOrDefault(e => e.UrlSlug.Equals(eventUrlSlug));

		        if (eventEntity == null)
		        {
			        return HttpNotFound();
		        }

		        var riderEntity = eventEntity.EventRiders.SingleOrDefault(r => r.UrlSlug.Equals(riderUrlSlug));

		        if (riderEntity == null)
		        {
			        return HttpNotFound();
		        }

		        var model = new RiderDetailViewModel
		        {
			        EventName = eventEntity.Name,
					PossessiveRiderName = riderEntity.PossessiveName,
					RiderName = riderEntity.Name,
					RiderStart = riderEntity.Start
		        };

				return View("Index", model);
	        }
        }
    }
}