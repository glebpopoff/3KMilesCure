using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DonationPortal.Engine;
using DonationPortal.Engine.Rider;
using DonationPortal.Engine.Social;
using DonationPortal.Web.Attributes;
using DonationPortal.Web.ViewModels;
using DonationPortal.Web.ViewModels.RiderDetail;
using LinqToTwitter;

namespace DonationPortal.Web.Controllers
{
	[ConditionalRequireHttps]
    public class RiderDetailController : Controller
    {

		private readonly EventRiderLocationProvider _locationProvider;
	    private readonly EventRiderMessageProvider _messageProvider;
	    private readonly ISocialFeedProvider _twitterFeedProvider;
        private readonly ISocialFeedProvider _facebookFeedProvider;

	    public RiderDetailController()
	    {
		    _messageProvider = new EventRiderMessageProvider();
		    _locationProvider = new EventRiderLocationProvider();

            _twitterFeedProvider = new ErrorHandlingSocialFeedProvider(new TwitterFeedProvider(
                ConfigurationManager.AppSettings["TwitterOAuthToken"],
                ConfigurationManager.AppSettings["TwitterOAuthTokenSecret"],
                ConfigurationManager.AppSettings["TwitterconsumerKey"],
                ConfigurationManager.AppSettings["TwitterconsumerSecret"]
            ));

            _facebookFeedProvider = new ErrorHandlingSocialFeedProvider(new FacebookFeedProvider(
              ConfigurationManager.AppSettings["FacebookPageId"],
              ConfigurationManager.AppSettings["FacebookAccessToken"]
          ));
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

                var twitterItems = _twitterFeedProvider.GetItems(riderEntity.EventRiderID, 10);

                var facebookItems = _facebookFeedProvider.GetItems(riderEntity.EventRiderID, 10);

                var socialItems = twitterItems.Union(facebookItems);

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
					Timer = new TimerViewModel(riderEntity.DurationGoal, riderEntity.End, riderEntity.Start),
					RecentMessages = _messageProvider.GetMessages(riderEntity.EventRiderID, 5),
                    SocialFeedItems = socialItems.OrderByDescending(i => i.Posted).Take(10).ToList(),
					DonateButtonText = riderEntity.DonateButtonText
		        };

				return View("Index", model);
	        }
        }
    }
}