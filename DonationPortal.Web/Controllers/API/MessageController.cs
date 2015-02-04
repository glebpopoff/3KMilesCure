using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DonationPortal.Engine;
using DonationPortal.Engine.Messages;
using DonationPortal.Engine.Rider;
using DonationPortal.Web.ApiModels.Messages;
using DotSpatial.Positioning;
using System;

namespace DonationPortal.Web.Controllers.API
{
	[RoutePrefix("api/v1")]
	public class MessageController : ApiController
	{
		private readonly IMessageLocationFilter _messageLocationFilter;
		private readonly EventRiderMessageProvider _messageProvider;

		public MessageController()
		{
			this._messageLocationFilter = new DistanceMessageLocationFilter(new Distance(50, DistanceUnit.Meters));
			this._messageProvider = new EventRiderMessageProvider();
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
					.Select(d => new DonationPortal.Web.ApiModels.Messages.RecentMessage()
					{
						DateReceived = d.DateReceived,
						Username = d.RiderMessageDonation.FirstName + " " + d.RiderMessageDonation.LastName,
						Message = d.RiderMessageDonation.Message
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
					
					// otherwise, store that we sent the message to the rider.
					entities.RecentMessages.Add(new DonationPortal.Engine.RecentMessage
					{
						DateReceived = DateTime.Now, 
						DonationID = donationMessage.ID
					});
				}

				entities.SaveChanges();

				return Request.CreateResponse(HttpStatusCode.OK, donationMessages);
			}
		}
	}
}