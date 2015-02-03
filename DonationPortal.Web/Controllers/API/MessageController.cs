using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DonationPortal.Engine;
using DonationPortal.Engine.Messages;
using DonationPortal.Web.ApiModels.Messages;
using DotSpatial.Positioning;
using System;

namespace DonationPortal.Web.Controllers.API
{
	[RoutePrefix("api/v1")]
	public class MessageController : ApiController
	{
		private readonly IMessageLocationFilter _messageLocationFilter;

		public MessageController()
		{
			this._messageLocationFilter = new DistanceMessageLocationFilter(new Distance(50, DistanceUnit.Meters));
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
                var recentMessages = entities.RecentMessages
                    .OrderByDescending(d => d.DateReceived)
                    .Take(messageCount)
                    .Select(d => new DTO.RecentMessage() {
                         DateReceived = d.DateReceived,
                         Username = d.RiderMessageDonation.FirstName + " " + d.RiderMessageDonation.LastName,
                         Message = d.RiderMessageDonation.Message
                    })
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, recentMessages);
            }
        }

		// this method isn't very restful...
		[Route("events/{eventSlug}/riders/{riderSlug}/messages/near")]
		[HttpPost]
		public HttpResponseMessage GetNearbyMessages(string eventSlug, string riderSlug, [FromBody]IEnumerable<Location> locations)
		{
			using (var entities = new DonationPortalEntities())
			{
				var donations = entities.RiderMessageDonations
					.Where(d => d.EventRider.UrlSlug.Equals(riderSlug) && d.EventRider.Event.UrlSlug.Equals(eventSlug)).ToList()
					.Where(d => _messageLocationFilter.IsMatch(d, locations)).Select(d => new DonationMessage
					{
						ID = d.DonationID,
						Latitude = (float) d.Latitude,
						Longitude = (float) d.Longitude,
						Message = d.Message,
						Sender = d.FirstName + " " + d.LastName
					});

                foreach (DonationMessage dm in donations)
                {
                    var recentMessages = entities.RecentMessages.Where(d => d.DonationID == dm.ID).Any();
                    if (!recentMessages)
                    {
                        var tempMess = new RecentMessage();
                        tempMess.DateReceived = DateTime.Now;
                        tempMess.DonationID = dm.ID;
                        entities.RecentMessages.Add(tempMess);
                    }
                    entities.SaveChanges();
                }



				return Request.CreateResponse(HttpStatusCode.OK, donations);
			}
		}
	}
}