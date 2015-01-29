using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DonationPortal.Web.ViewModels.Home;

namespace DonationPortal.Web.ViewModels.Template
{
	public class HeaderViewModel
	{
		public IEnumerable<EventViewModel> Riders { get; set; }
		public string FeaturedEventTitle { get; set; }
	}
}