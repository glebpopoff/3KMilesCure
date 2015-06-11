using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToTwitter;
using System.Threading.Tasks;
using System.Configuration;
using DonationPortal.Engine;
using DonationPortal.Engine.Social;
using DonationPortal.Web.ViewModels.Social;

namespace DonationPortal.Web.Controllers
{
    public class SocialController : Controller
    {
	    private readonly ISocialFeedProvider _twitterFeedProvider;
        private readonly ISocialFeedProvider _facebookFeedProvider;

	    public SocialController()
        {
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
		
        public ActionResult Index(string eventUrlSlug, string riderUrlSlug)
        {

	        using (var entities = new DonationPortalEntities())
	        {
		        var @event = entities.Events.SingleOrDefault(e => e.UrlSlug == eventUrlSlug);

		        if (@event == null)
		        {
			        return HttpNotFound();
		        }

		        var rider = @event.EventRiders.SingleOrDefault(r => r.UrlSlug == riderUrlSlug);

				if (rider == null)
				{
					return HttpNotFound();
				}

		        var twitterItems = _twitterFeedProvider.GetItems(rider.EventRiderID,100);

                //var facebookItems = _facebookFeedProvider.GetItems(rider.EventRiderID, 100);

                var items = twitterItems;//.Union(facebookItems);

		        var model = new SocialViewModel
		        {
					EventName = @event.Name,
					RiderName = rider.Name,
					Items = items.OrderByDescending(i=> i.Posted).Take(100).ToList()
		        };

                return View("Index", model);
            }
        }
    }
}