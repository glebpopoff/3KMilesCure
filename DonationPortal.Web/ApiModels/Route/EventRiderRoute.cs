using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonationPortal.Web.ApiModels.Route
{
	public class EventRiderRoute
	{
		public int RouteID { get; set; }
		public string Name { get; set; }
		public string Color { get; set; }
		public string UrlSlug { get; set; }
		public IEnumerable<RouteVertex> Vertices { get; set; } 
	}
}