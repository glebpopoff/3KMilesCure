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
	    private readonly FeedProvider _feedProvider;

	    public SocialController()
	    {
		    _feedProvider = new FeedProvider(new SingleUserInMemoryCredentialStore
			{
				ConsumerKey = ConfigurationManager.AppSettings["TwitterconsumerKey"],
				ConsumerSecret = ConfigurationManager.AppSettings["TwitterconsumerSecret"],
				OAuthToken = ConfigurationManager.AppSettings["TwitterOAuthToken"],
				OAuthTokenSecret = ConfigurationManager.AppSettings["TwitterOAuthTokenSecret"]
		    });
	    }
		
        public async Task<ActionResult> Index(string eventUrlSlug, string riderUrlSlug)
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

		        var items = await _feedProvider.GetItems(rider.EventRiderID);

		        var model = new SocialViewModel
		        {
					EventName = @event.Name,
					RiderName = rider.Name,
					Items = items
		        };

                return View("Index", model);
            }
        }
    }
}