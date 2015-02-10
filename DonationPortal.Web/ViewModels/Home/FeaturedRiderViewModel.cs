using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DonationPortal.Engine;

namespace DonationPortal.Web.ViewModels.Home
{
	public class FeaturedRiderViewModel
	{
		public string DetailUrl { get; set; }
		public string EventUrlSlug { get; set; }
		public string RiderUrlSlug { get; set; }
		public string EventName { get; set; }
		public string RiderName { get; set; }
		public DateTime RiderStart { get; set; }
		public IHtmlString RiderStory { get; set; }
		public string DistanceGoal { get; set; }
		public bool HasDistanceGoal
		{
			get { return !string.IsNullOrWhiteSpace(DistanceGoal); }
		}
		public string PossessiveRiderName { get; set; }
		public double TotalMiles { get; set; }
		
		public DateTime RiderEnd { get; set; }
		public IEnumerable<RecentMessage> RecentMessages { get; set; }

		public TimerViewModel Timer { get; set; }
	}
}