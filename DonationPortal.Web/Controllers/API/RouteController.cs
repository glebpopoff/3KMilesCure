using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DonationPortal.Engine;
using DonationPortal.Engine.Rider;
using DonationPortal.Web.ApiModels.Routes;
using RouteVertex = DonationPortal.Web.ApiModels.Routes.RouteVertex;
using DotSpatial.Positioning;
using System;
using System.Collections.Generic;
using System.Collections;
namespace DonationPortal.Web.Controllers.API
{
	[RoutePrefix("api/v1")]
    public class RouteController : ApiController
	{
		private readonly EventRiderLocationProvider _locationProvider;

		public RouteController()
		{
			_locationProvider = new EventRiderLocationProvider();
		}

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
					Vertices = route.RouteVertexes.OrderBy(v => v.Order).Select(vertex => new RouteVertex()
					{
						Latitude = (float)vertex.Latitude,
						Longitude = (float)vertex.Longitude
					}).ToList() // need to materialize now, otherwise the database will already be disposed when the message is serialized.
				}).ToList(); // see above

                if (routes.Count == 1 && rider.Routes.First().Circular == false)
                {
                    var mostRecentLocation = _locationProvider.GetLocation(rider.EventRiderID);
                    if (mostRecentLocation.HasValue)
                    {
                        var route = routes[0];
                        int minKey = 0;
                        int maxKey = route.Vertices.Count();
                        int currentTry = maxKey / 2;
                        var currentPos = new Position(new Latitude(route.Vertices.ElementAt(currentTry).Latitude), new Longitude(route.Vertices.ElementAt(currentTry).Longitude));
                        var startPos = new Position(new Latitude(route.Vertices.First().Latitude), new Longitude(route.Vertices.First().Longitude));
                        var endPos = new Position(new Latitude(route.Vertices.Last().Latitude), new Longitude(route.Vertices.Last().Longitude));
                        var currentBestDistance = mostRecentLocation.Value.DistanceTo(startPos).Value;


                        while (maxKey != minKey)
                        {
                            currentTry = (minKey + maxKey) / 2;
                            RouteVertex vertex = route.Vertices.ElementAt(currentTry);
                            var pos = new Position(new Latitude(vertex.Latitude), new Longitude(vertex.Longitude));
                            var currentDistance = pos.DistanceTo(mostRecentLocation.Value).ToMeters().Value;
                            if (currentDistance < currentBestDistance)
                            {
                                if (pos.DistanceTo(startPos).Value < pos.DistanceTo(endPos).Value)
                                {
                                    endPos = pos;
                                    maxKey = currentTry;
                                }
                                else
                                {
                                    startPos = pos;
                                    minKey = currentTry;
                                }
                               currentBestDistance = currentDistance;
                            }
                            else
                            {
                                if (mostRecentLocation.Value.DistanceTo(startPos).Value < mostRecentLocation.Value.DistanceTo(endPos).Value)
                                {
                                    endPos = pos;
                                    maxKey = currentTry;
                                }
                                else
                                {
                                    startPos = pos;
                                    minKey = currentTry;
                                }
                            }
                        }

                       
                        route.VisitedVertices = route.Vertices.Take(currentTry).ToList();
                       
                        route.UnvisitedVertices = route.Vertices.Skip(currentTry).ToList();
                       
                        route.Vertices = null; //Save on response data
                    }
                }


				return Request.CreateResponse(HttpStatusCode.OK, routes);
			}
	    }

		[Route("events/{eventSlug}/riders/{riderSlug}/location")]
		[HttpGet]
		public HttpResponseMessage GetEventRiderLocation(string eventSlug, string riderSlug)
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

				var mostRecentLocation = _locationProvider.GetLocation(rider.EventRiderID);

				if (!mostRecentLocation.HasValue)
				{
					return Request.CreateResponse(HttpStatusCode.NoContent);
				}

				var totalDistance = _locationProvider.GetTotalDistance(rider.EventRiderID);

				return Request.CreateResponse(HttpStatusCode.OK, new CurrentLocation()
				{
                    EventRiderID = rider.EventRiderID,
					Latitude = mostRecentLocation.Value.Latitude.DecimalDegrees,
					Longitude = mostRecentLocation.Value.Longitude.DecimalDegrees,
					TotalMiles = totalDistance.ToStatuteMiles().Value
				});
			}
		}
    }
}
