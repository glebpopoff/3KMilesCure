using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationPortal.Engine;
using DonationPortal.Engine.Rider;
using DonationPortal.Web.ViewModels.Home;
using System.Data.Entity;
using DonationPortal.Web.ViewModels;

namespace DonationPortal.Web.Controllers
{
    public class HomeController : Controller
    {
		private readonly EventRiderLocationProvider _locationProvider;
	    private readonly EventRiderMessageProvider _messageProvider;

		public HomeController()
		{
			_locationProvider = new EventRiderLocationProvider();
			_messageProvider = new EventRiderMessageProvider();
		}

        // GET: Home
        public ActionResult Index()
        {
	        using (var entities = new DonationPortalEntities())
	        {
		        var urlHelper = new UrlHelper(this.ControllerContext.RequestContext);

		        var featuredRider = entities.EventRiders.Include(r => r.Event).Single(r => r.IsFeatured); //rob

		        var model = new HomeViewModel
		        {
					FeaturedRider = new FeaturedRiderViewModel
					{
						EventName = featuredRider.Event.Name,
						EventUrlSlug = featuredRider.Event.UrlSlug,
						RiderName = featuredRider.Name,
						RiderStart = featuredRider.Start,
						RiderEnd = featuredRider.End,
						RiderUrlSlug = featuredRider.UrlSlug,
						RiderStory = new HtmlString(featuredRider.Story),
						DistanceGoal = featuredRider.DistanceGoal,
						DetailUrl = urlHelper.Action("Index", "RiderDetail", new { EventUrlSlug = featuredRider.Event.UrlSlug, RiderUrlSlug = featuredRider.UrlSlug }),
						PossessiveRiderName = featuredRider.PossessiveName,
						TotalMiles = _locationProvider.GetTotalDistance(featuredRider.EventRiderID).ToStatuteMiles().Value,
						RecentMessages = _messageProvider.GetMessages(featuredRider.EventRiderID, 5),
						Timer = new TimerViewModel(featuredRider.DurationGoal, featuredRider.End, featuredRider.Start)
					},
					Riders = entities.EventRiders.Include(r => r.Event).Include(r => r.RiderMessageDonations).Where(r => r.HideFromSite == false).ToList().Select(rider => new RiderViewModel
					{
						DonationGoal = rider.DonationGoal,
						EventName = rider.Event.Name,
						EventUrlSlug = rider.Event.UrlSlug,
						RiderName = rider.Name,
						RiderDescription = new HtmlString(rider.Teaser),
						RiderUrlSlug = rider.UrlSlug,
						TotalRaised = rider.RiderMessageDonations.Sum(d => d.Amount),
						PossessiveRiderName = rider.PossessiveName,
						DetailUrl = urlHelper.Action("Index", "RiderDetail", new { EventUrlSlug = rider.Event.UrlSlug, RiderUrlSlug = rider.UrlSlug })
					}).OrderBy(r => Guid.NewGuid())
				};

				return View("Index", model);
	        }
        }
    }
}