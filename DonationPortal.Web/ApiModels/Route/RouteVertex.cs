using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonationPortal.Web.ApiModels.Route
{
	public class RouteVertex
	{
		public float Latitude { get; set; }
		public float Longitude { get; set; }
		public int Order { get; set; }
	}
}