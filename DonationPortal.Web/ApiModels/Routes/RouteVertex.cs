using System;
using System.Collections.Generic;
using DotSpatial.Positioning;
namespace DonationPortal.Web.ApiModels.Routes
{
	public class RouteVertex
	{
        public RouteVertex()
        {

        }

        public RouteVertex(float lat, float lon)
        {
            Latitude = lat;
            Longitude = lon;
        }
		public float Latitude { get; set; }
		public float Longitude { get; set; }
	}
}