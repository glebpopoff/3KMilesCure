using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationPortal.Engine;
using DonationPortal.Engine.Rider;
using DonationPortal.Web.ViewModels;
using DonationPortal.Web.ViewModels.RiderDetail;

namespace DonationPortal.Web.Controllers
{
    public class RiderDetailController : Controller
    {

		private readonly EventRiderLocationProvider _locationProvider;

	    public RiderDetailController()
	    {
		    _locationProvider = new EventRiderLocationProvider();
	    }

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
					RiderStart = riderEntity.Start,
					DonationGoal = riderEntity.DonationGoal,
					DonationTotal = riderEntity.RiderMessageDonations.Sum(r => r.Amount),
					RiderEnd = riderEntity.End,
					DurationGoal = riderEntity.DurationGoal,
					MilesGoal = riderEntity.DistanceGoal,
					MilesTravelled = (int)_locationProvider.GetTotalDistance(riderEntity.EventRiderID).ToStatuteMiles().Value,
					DonationStart = riderEntity.DonationStart,
					Teaser = new HtmlString(riderEntity.DetailTeaser),
					Pronoun = riderEntity.Pronoun,
					HeroImageText = riderEntity.DetailHeroText,
					HeroImageUri = riderEntity.DetailHeroImage,
					RouteDescription = new HtmlString(riderEntity.RouteDescription),
					DonationDescription = new HtmlString(riderEntity.DonationDescription),
					ChooseLocationText = new HtmlString(riderEntity.ChooseLocationText),
					ShortEventName = eventEntity.ShortName,
					EventUrlSlug = eventUrlSlug,
					RiderUrlSlug = riderUrlSlug,
					Timer = new TimerViewModel(riderEntity.DurationGoal, riderEntity.End, riderEntity.Start)
		        };

				return View("Index", model);
	        }
        }
    }
}