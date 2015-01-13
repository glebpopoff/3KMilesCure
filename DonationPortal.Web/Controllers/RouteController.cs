using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DonationPortal.Engine;
using DonationPortal.Web.ApiModels.Route;

namespace DonationPortal.Web.Controllers
{
	[RoutePrefix("api/v1")]
    public class RouteController : ApiController
    {
		[Route("events/{eventSlug}/riders/{riderSlug}/routes")]
		[HttpGet]
	    public HttpResponseMessage GetEventRiderRoutes(string eventSlug, string riderSlug)
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

				var routes = rider.Routes.Select(route => new EventRiderRoute()
				{
					Name = route.Name,
					RouteID = route.RouteID,
					Color = '#' + route.Color,
					UrlSlug = route.UrlSlug,
					Vertices = route.RouteVertexes.OrderBy(v => v.Order).Select(vertex => new ApiModels.Route.RouteVertex()
					{
						Latitude = (float)vertex.Latitude,
						Longitude = (float)vertex.Longitude
					}).ToList() // need to materialize now, otherwise the database will already be disposed when the message is serialized.
				}).ToList(); // see above

				return Request.CreateResponse(HttpStatusCode.OK, routes);
			}
	    }
    }
}
