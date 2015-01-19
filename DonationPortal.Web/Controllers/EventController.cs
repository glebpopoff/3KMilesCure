using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DonationPortal.Engine;
using EventRider = DonationPortal.Web.ApiModels.EventRiders.EventRider;

namespace DonationPortal.Web.Controllers
{
	[RoutePrefix("api/v1")]
    public class EventController : ApiController
	{
		[Route("events")]
		[HttpGet]
		public HttpResponseMessage GetEvents()
		{
			using (var entities = new DonationPortalEntities())
			{
				var events = entities.Events.Select(@event => new ApiModels.Events.Event
				{
					EventID = @event.EventID,
					Name = @event.Name,
					UrlSlug = @event.UrlSlug
				}).ToList();

				return Request.CreateResponse(HttpStatusCode.OK, events);
			}
		}
    }
}
