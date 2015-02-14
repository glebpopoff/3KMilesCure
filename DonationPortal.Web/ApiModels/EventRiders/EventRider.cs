using System;

namespace DonationPortal.Web.ApiModels.EventRiders
{
	public class EventRider
	{
		public int EventRiderID { get; set; }
		public string Name { get; set; }
		public string UrlSlug { get; set; }
		public float MapLatitude { get; set; }
		public float MapLongitude { get; set; }
		public int MapZoom { get; set; }
		public float MarkerLatitude { get; set; }
		public float MarkerLongitude { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
	}
}