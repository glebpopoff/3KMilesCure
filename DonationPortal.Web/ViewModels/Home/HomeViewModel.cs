using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonationPortal.Web.ViewModels.Home
{
	public class HomeViewModel
	{
		public FeaturedRiderViewModel FeaturedRider { get; set; }
		public IEnumerable<RiderViewModel> Riders { get; set; }
	}
}