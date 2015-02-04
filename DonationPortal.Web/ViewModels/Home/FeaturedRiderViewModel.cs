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
		public string DurationGoal { get; set; }
		public bool HasDurationGoal
		{
			get { return !string.IsNullOrWhiteSpace(DurationGoal); }
		}
		public string DistanceGoal { get; set; }
		public bool HasDistanceGoal
		{
			get { return !string.IsNullOrWhiteSpace(DistanceGoal); }
		}
		public string PossessiveRiderName { get; set; }
		public double TotalMiles { get; set; }

		public TimeSpan ElapsedTime
		{
			get
			{
				if (DateTime.Now < RiderStart)
				{
					return new TimeSpan();
				}

				if (DateTime.Now > RiderEnd)
				{
					return RiderEnd - RiderStart;
				}

				return DateTime.Now - RiderStart;
			}
		}

		public DateTime RiderEnd { get; set; }
		public IEnumerable<RecentMessage> RecentMessages { get; set; }
	}
}