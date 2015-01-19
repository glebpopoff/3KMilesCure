using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonationPortal.Web.ApiModels.Events
{
	public class Event
	{
		public int EventID { get; set; }
		public string Name { get; set; }
		public string UrlSlug { get; set; }
	}
}