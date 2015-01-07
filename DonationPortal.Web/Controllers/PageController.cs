using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationPortal.Web.ViewModels.Page;

namespace DonationPortal.Web.Controllers
{
    public class PageController : Controller
    {
		/// <summary>
		/// This controller will handle routing to the appropriate angular app.
		/// We only have one right now.
		/// </summary>
		/// <returns></returns>
        public ActionResult Index()
		{
			var model = new PageViewModel
			{
				AngularAppMainScript = "/ui/app/homepage/js/main",
				AngularAppName = "homepageApp"
			};

            return View("Index", model);
        }
    }
}