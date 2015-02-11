using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonationPortal.Web.ViewModels.Social
{
	public class SocialViewModel
	{
		public string EventName { get; set; }
		public string RiderName { get; set; }
		public IEnumerable<DonationPortal.Engine.Social.SocialFeedItem> Items { get; set; }
	}
}