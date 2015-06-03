using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DonationPortal.Engine;
using DonationPortal.Engine.Messages;
using DonationPortal.Engine.Rider;
using DonationPortal.Web.ApiModels.Messages;
using DonationPortal.Web.ApiModels.Routes;
using DonationPortal.Web.Hubs;
using DotSpatial.Positioning;
using System;
using log4net;
using Microsoft.AspNet.SignalR;
using RecentMessage = DonationPortal.Engine.RecentMessage;

namespace DonationPortal.Web.Controllers.API
{
	[RoutePrefix("api/v1")]
	public class MessageController : ApiController
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof (MessageController));

		private readonly EventRiderLocationProvider _eventRiderLocationProvider;
		private readonly IMessageLocationFilter _messageLocationFilter;
		private readonly EventRiderMessageProvider _messageProvider;

		public MessageController()
		{
			this._messageLocationFilter = new DistanceMessageLocationFilter(new Distance(int.Parse(ConfigurationManager.AppSettings["MessageRadiusMeters"]), DistanceUnit.Meters));
			this._messageProvider = new EventRiderMessageProvider();
			this._eventRiderLocationProvider = new EventRiderLocationProvider();
		}

		[Route("events/{eventSlug}/riders/{riderSlug}/messages")]
		[HttpGet]
		public HttpResponseMessage GetMessagesForRider(string eventSlug, string riderSlug)
		{
			// should we return 404's for events or riders that don't exist?

			using (var entities = new DonationPortalEntities())
			{
				var donations =
					entities.RiderMessageDonations.Where(
						d => d.EventRider.UrlSlug.Equals(riderSlug) && d.EventRider.Event.UrlSlug.Equals(eventSlug)).Select(d => new DonationMessage
						{
							ID = d.DonationID,
							Latitude = (float)d.Latitude,
							Longitude = (float)d.Longitude,
							Message = d.Message,
							Sender = d.FirstName + " " + d.LastName
						}).ToList();

				return Request.CreateResponse(HttpStatusCode.OK, donations);
			}
		}

		[Route("events/{eventSlug}/riders/{riderSlug}/messages/recent")]
		[HttpGet]
		public HttpResponseMessage GetRecentMessages(string eventSlug, string riderSlug, int messageCount)
		{
			using (var entities = new DonationPortalEntities())
			{
				var rider = entities.EventRiders.SingleOrDefault(r => Equals(r.UrlSlug, riderSlug) && Equals(r.Event.UrlSlug, eventSlug));

				var messages = _messageProvider.GetMessages(rider.EventRiderID, messageCount)
					.Select(d => new ApiModels.Messages.RecentMessage()
					{
						DateReceived = d.DateReceived,
						Username = d.RiderMessageDonation.FirstName + " " + d.RiderMessageDonation.LastName,
						Message = d.RiderMessageDonation.Message,
                        Amount = d.RiderMessageDonation.Amount
					})
					.ToList();

				return Request.CreateResponse(HttpStatusCode.OK, messages);
			}
		}

		// this method isn't very restful...
		[Route("events/{eventSlug}/riders/{riderSlug}/messages/near")]
		[HttpPost]
		public HttpResponseMessage GetNearbyMessages(string eventSlug, string riderSlug, [FromBody]DonationPortal.Engine.Messages.LocationVisit[] locationVisits)
		{
			_log.DebugFormat("Request for messages near any of the following locations for {0} {1}.", eventSlug, riderSlug);

			using (var entities = new DonationPortalEntities())
			{
				// grab the rider that we've been asked to retrieve messages for.
				var rider = entities.EventRiders.SingleOrDefault(r => Equals(r.UrlSlug, riderSlug) && Equals(r.Event.UrlSlug, eventSlug));

				// if we can't find them, bail out.
				if (rider == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound,
						string.Format("Rider {0} for event {1} not found.", riderSlug, eventSlug));
				}

				// first, store the locations we've been sent as places the user has visited.
				foreach (var location in locationVisits)
				{
					entities.LocationVisits.Add(new DonationPortal.Engine.LocationVisit
					{
						DateVisited = location.Date,
						EventRider = rider,
						Latitude = location.Latitude,
						Longtitude = location.Longitude
					});
				}

				// then determine which messages apply to the locations visited
				var donationMessages = entities.RiderMessageDonations
					.Where(d => d.EventRider.UrlSlug.Equals(riderSlug) && d.EventRider.Event.UrlSlug.Equals(eventSlug)).ToList()
					.Where(d => _messageLocationFilter.IsMatch(d, locationVisits)).Select(d => new DonationMessage
					{
						ID = d.DonationID,
						Latitude = (float)d.Latitude,
						Longitude = (float)d.Longitude,
						Message = d.Message,
						Sender = d.FirstName + " " + d.LastName
					}).ToList();

				// finally, store a reference to the messages we are passing back as having been received by the rider.
				foreach (var donationMessage in donationMessages)
				{
					// check if we've already sent this message before
					var recentMessages = entities.RecentMessages.Any(d => d.DonationID == donationMessage.ID);

					// if we have, no need to store it again.
					if (recentMessages)
					{
						continue;
					}

					var message = new RecentMessage
					{
						DateReceived = DateTime.Now,
						DonationID = donationMessage.ID
					};
					
					// otherwise, store that we sent the message to the rider.
					entities.RecentMessages.Add(message);

					// push the message to all listening clients
					NotifyRecentMessage(rider, message);
				}

				entities.SaveChanges();

				NotifyCurrentLocation(rider);

				return Request.CreateResponse(HttpStatusCode.OK, donationMessages);
			}
		}

		private static void NotifyRecentMessage(EventRider rider, RecentMessage message)
		{
			var context = GlobalHost.ConnectionManager.GetHubContext<EventRiderMessageHub>();

			context.Clients.All.addRecentMessage(new EventRiderRecentMessage()
			{
				DateReceived = message.DateReceived,
				EventRiderID = rider.EventRiderID,
				Sender = message.RiderMessageDonation.FirstName,
				Text = message.RiderMessageDonation.Message,
                Amount = message.RiderMessageDonation.Amount
			});
		}

		private void NotifyCurrentLocation(EventRider rider)
		{
			var mostRecentLocation = _eventRiderLocationProvider.GetLocation(rider.EventRiderID);

			if (!mostRecentLocation.HasValue)
			{
				return;
			}

			var totalDistance = _eventRiderLocationProvider.GetTotalDistance(rider.EventRiderID);

			var context = GlobalHost.ConnectionManager.GetHubContext<EventRiderLocationHub>();

			context.Clients.All.updateLocation(new CurrentLocation()
			{
                EventRiderID = rider.EventRiderID,
				Latitude = mostRecentLocation.Value.Latitude.DecimalDegrees,
				Longitude = mostRecentLocation.Value.Longitude.DecimalDegrees,
				TotalMiles = totalDistance.ToStatuteMiles().Value
			});
		}
	}
}