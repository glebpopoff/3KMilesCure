using DonationPortal.Web.ApiModels.Social;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DonationPortal.Web.Controllers
{
    public class SocialController : Controller
    {
        // GET: Social Dashboard
        public ActionResult Index()
        {
            List<SocialFeedItem> items = new List<SocialFeedItem>();
            return View(items);
        }
    }
}