using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonationPortal.Web.ViewModels.Home
{
	public class RiderViewModel
	{
		public string EventName { get; set; }
		public string RiderName { get; set; }
		public string EventUrlSlug { get; set; }
		public string RiderUrlSlug { get; set; }
		public IHtmlString RiderDescription { get; set; }
		public decimal TotalRaised { get; set; }
		public decimal DonationGoal { get; set; }
		public string PossessiveRiderName { get; set; }
		public string DetailUrl { get; set; }
	}
}