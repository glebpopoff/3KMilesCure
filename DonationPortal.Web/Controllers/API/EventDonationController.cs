using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DonationPortal.Engine;
using DonationPortal.Engine.PaymentProcessor;
using DonationPortal.Web.ApiModels.EventDonations;

namespace DonationPortal.Web.Controllers.API
{
	[RoutePrefix("api/v1")]
    public class EventDonationController : ApiController
    {
		private readonly IImmediatePaymentProcessor _immediatePaymentProcessor;

		public EventDonationController()
		{
			_immediatePaymentProcessor = new MockImmediatePaymentProcessor();
		}
		
		[Route("events/{eventSlug}/riders/{riderSlug}/donations")]
		[HttpPost]
		public HttpResponseMessage AddDonationForRider(string eventSlug, string riderSlug, RiderDonation donation)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			// 404 or 400's for events/riders that don't exist?

			var paymentResult = _immediatePaymentProcessor.Process(new ImmediatePaymentRequest
			{
				Amount = donation.DonationAmount,
				City = donation.City,
				CvvNumber = donation.CvvNumber,
				Email = donation.Email,
				ExpirationMonth = donation.ExpirationMonth,
				ExpirationYear = donation.ExpirationYear,
				FirstName = donation.FirstName,
				LastName = donation.LastName,
				State = donation.State,
				StreetAddress1 = donation.StreetAddress1,
				StreetAddress2 = donation.StreetAddress2,
				ZipCode = donation.ZipCode,
				CreditCardNumber = donation.CreditCardNumber
			});

			using (var entities = new DonationPortalEntities())
			{
				entities.RiderMessageDonations.Add(new RiderMessageDonation
				{
					City = donation.City,
					Email = donation.Email,
					FirstName = donation.FirstName,
					LastName = donation.LastName,
					Latitude = donation.Latitude,
					Longitude = donation.Longitude,
					Message = donation.Message,
					State = donation.State,
					StreetAddress1 = donation.StreetAddress1,
					StreetAddress2 = donation.StreetAddress2,
					TransactionID = paymentResult.TransactionID,
					ZipCode = donation.ZipCode,
					EventRider = entities.Events.Single(e => e.UrlSlug.Equals(eventSlug)).EventRiders.Single(r => r.UrlSlug.Equals(riderSlug)),
					Amount = donation.DonationAmount
				});

				entities.SaveChanges();
			}

			return Request.CreateResponse(HttpStatusCode.Created, donation);
		}
    }
}
