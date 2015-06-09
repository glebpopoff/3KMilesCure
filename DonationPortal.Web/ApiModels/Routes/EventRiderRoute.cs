using System.Collections.Generic;

namespace DonationPortal.Web.ApiModels.Routes
{
	public class EventRiderRoute
	{
		public int RouteID { get; set; }
		public string Name { get; set; }
		public string Color { get; set; }
		public string UrlSlug { get; set; }
		public IEnumerable<RouteVertex> Vertices { get; set; }
        public List<RouteVertex> VisitedVertices { get; set; }
        public List<RouteVertex> UnvisitedVertices { get; set; } 
	}
}