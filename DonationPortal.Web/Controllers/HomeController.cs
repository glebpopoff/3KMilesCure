using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationPortal.Engine;
using DonationPortal.Web.ViewModels.Home;
using System.Data.Entity;

namespace DonationPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
	        using (var entities = new DonationPortalEntities())
	        {
		        var urlHelper = new UrlHelper(this.ControllerContext.RequestContext);

				// todo, flag someone as the featured one
		        var featuredRider = entities.EventRiders.Include(r => r.Event).First();

		        var model = new HomeViewModel
		        {
					FeaturedRider = new FeaturedRiderViewModel
					{
						EventName = featuredRider.Event.Name,
						EventUrlSlug = featuredRider.Event.UrlSlug,
						RiderName = featuredRider.Name,
						RiderStart = featuredRider.Start,
						RiderUrlSlug = featuredRider.UrlSlug,
						RiderStory = new HtmlString(featuredRider.Story),
						DurationGoal = featuredRider.DurationGoal,
						DistanceGoal = featuredRider.DistanceGoal,
						DetailUrl = urlHelper.Action("Index", "RiderDetail", new { EventUrlSlug = featuredRider.Event.UrlSlug, RiderUrlSlug = featuredRider.UrlSlug }),
						PossessiveRiderName = featuredRider.PossessiveName
					},
					Riders = entities.EventRiders.Include(r => r.Event).Include(r => r.RiderMessageDonations).ToList().Select(rider => new RiderViewModel
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
					})
				};

				return View("Index", model);
	        }
        }
    }
}