using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DonationPortal.Engine;
using EventRider = DonationPortal.Web.ApiModels.EventRiders.EventRider;

namespace DonationPortal.Web.Controllers.API
{
	[RoutePrefix("api/v1")]
    public class EventRiderController : ApiController
    {
		[Route("events/{eventSlug}/riders")]
		[HttpGet]
		public HttpResponseMessage GetEventRiders(string eventSlug)
		{
			using (var entities = new DonationPortalEntities())
			{
				var eventEntity = entities.Events.SingleOrDefault(e => e.UrlSlug.Equals(eventSlug));

				if (eventEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Event {0} not found.", eventSlug));
				}

				var riders = eventEntity.EventRiders.Select(rider => new EventRider
				{
					EventRiderID = rider.EventRiderID,
					Name = rider.Name,
					UrlSlug = rider.UrlSlug,
					MapLatitude = (float) rider.MapLatitude,
					MapLongitude = (float) rider.MapLongitude,
					MapZoom = rider.MapZoom,
					MarkerLatitude = (float) rider.MarkerLatitude,
					MarkerLongitude = (float) rider.MarkerLongitude
				}).ToList();

				return Request.CreateResponse(HttpStatusCode.OK, riders);
			}
		}

		[Route("events/{eventSlug}/riders/{riderSlug}")]
		[HttpGet]
	    public HttpResponseMessage GetEventRider(string eventSlug, string riderSlug)
		{
			using (var entities = new DonationPortalEntities())
		    {
			    var eventEntity = entities.Events.SingleOrDefault(e => e.UrlSlug.Equals(eventSlug));

			    if (eventEntity == null)
			    {
				    return Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Event {0} not found.", eventSlug));
			    }

			    var rider = eventEntity.EventRiders.SingleOrDefault(r => r.UrlSlug.Equals(riderSlug));

			    if (rider == null)
			    {
				    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
					    string.Format("Rider {0} not found for event {1}.", riderSlug, eventSlug));
			    }

				return Request.CreateResponse(HttpStatusCode.OK, new EventRider
			    {
					EventRiderID = rider.EventRiderID,
					Name = rider.Name,
					UrlSlug = rider.UrlSlug,
					MapLatitude = (float)rider.MapLatitude,
					MapLongitude = (float)rider.MapLongitude,
					MapZoom = rider.MapZoom,
					MarkerLatitude = (float)rider.MarkerLatitude,
					MarkerLongitude = (float)rider.MarkerLongitude
			    });
		    }
	    }
    }
}