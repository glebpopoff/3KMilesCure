using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationPortal.Engine;
using DonationPortal.Engine.PaymentProcessor;
using DonationPortal.Web.Attributes;
using DonationPortal.Web.ViewModels.Donation;
using System.Net;
using System.IO;
using log4net;
using System.Data.Entity.Validation;
namespace DonationPortal.Web.Controllers
{
	[ConditionalRequireHttps]
	public class DonationController : Controller
	{
		private const string SuccessfulDonationKey = "successful_donation";
        private static readonly ILog _log = LogManager.GetLogger(typeof(DonationController));

		private readonly IImmediatePaymentProcessor _immediatePaymentProcessor;
		private readonly IRecurringPaymentProcessor _recurringPaymentProcessor;

		public DonationController()
		{
			this._immediatePaymentProcessor = new PaypalImmediatePaymentProcessor(new CreditCardIssuerDetector());
			this._recurringPaymentProcessor = new MockRecurringPaymentProcessor();
		}

		[HttpGet]
		public ActionResult Index()
		{
			var model = new DonationViewModel
			{
				HasSuccessfulDonation = TempData[SuccessfulDonationKey] != null && (bool)TempData[SuccessfulDonationKey]
			};

			return View("Index", model);
		}


        [HttpPost]
        [Route("/Donation/Complete")]
        public ActionResult Complete()
        {
            WebRequest req = WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["PayPalUrl"] + "?cmd=_notify-validate&" + Request.Form.ToString());
            WebResponse resp = req.GetResponse();
            Stream dataStream = resp.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content. 
            string responseFromServer = reader.ReadToEnd();
            if (responseFromServer.Equals("VERIFIED") && Request.Form["payment_status"].Equals("Completed"))
            {
                string encodedCookie = HttpUtility.UrlDecode(Request.Cookies["donation"].Value);

                var originalSubmission = System.Web.Helpers.Json.Decode(encodedCookie);
                string eventSlug = originalSubmission.eventSlug;
                string riderSlug = originalSubmission.riderSlug;
                try
                {
                    using (var entities = new DonationPortalEntities())
                    {
                        var @event = entities.Events.SingleOrDefault(e => e.UrlSlug.Equals(eventSlug));


                        var rider = @event.EventRiders.SingleOrDefault(r => r.UrlSlug.Equals(riderSlug));

                        entities.RiderMessageDonations.Add(new RiderMessageDonation
                        {
                            City = Request.Form["address_city"],
                            Email = Request.Form["payer_email"],
                            FirstName = Request.Form["first_name"],
                            LastName = Request.Form["last_name"],
                            Latitude = Double.Parse(originalSubmission.Latitude),
                            Longitude = Double.Parse(originalSubmission.Longitude),
                            Message = originalSubmission.Message,
                            State = Request.Form["address_state"],
                            StreetAddress1 = Request.Form["address_street"],
                            StreetAddress2 = "",
                            TransactionID = Request.Form["txn_id"],
                            PaymentResource = Request.Form["payer_id"],
                            ZipCode = Request.Form["address_zip"],
                            EventRider = rider,
                            Amount = Decimal.Parse(Request.Form["mc_gross"]),
                            Date = DateTime.Now
                        });


                        entities.SaveChanges();

                        Request.Cookies.Remove("donation");
                    }
                } 
                catch (DbEntityValidationException ex)
                {
                    _log.Error("Error adding rider via callback", ex);
                    if (ex.EntityValidationErrors != null)
                    {
                        foreach (DbEntityValidationResult result in ex.EntityValidationErrors)
                        {
                            if (result.ValidationErrors != null)
                            {
                                foreach (DbValidationError error in result.ValidationErrors)
                                {
                                    _log.Error(error.PropertyName + ": " + error.ErrorMessage);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Error adding rider via callback", ex);
                }


                return View("Complete");
            }
            else
            {
                return View("Failue");
            }

            
        }

		[HttpPost]
        [Route("/Donation")]
		public ActionResult Index(DonationViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("Index", model);
			}

			string transactionID;
			string paymentResource;

			/*switch (model.DonationType)
			{
				case NonEventDonationType.OneTimeDonation:
					{*/
			var request = new ImmediatePaymentRequest
			{
				Amount = model.DonationAmount,
				City = model.City,
				CvvNumber = model.CvvNumber,
				Email = model.Email,
				ExpirationMonth = model.ExpirationMonth,
				ExpirationYear = model.ExpirationYear,
				FirstName = model.FirstName,
				LastName = model.LastName,
				State = model.State,
				StreetAddress1 = model.StreetAddress1,
				StreetAddress2 = model.StreetAddress2,
				ZipCode = model.ZipCode,
				CreditCardNumber = model.CreditCardNumber
			};

			var result = _immediatePaymentProcessor.Process(request);

			transactionID = result.TransactionID;
			paymentResource = result.PaymentResource;

			using (var entities = new DonationPortalEntities())
			{
				entities.Donations.Add(new Donation
				{
					Amount = model.DonationAmount,
					FirstName = model.FirstName,
					LastName = model.LastName,
					StreetAddress1 = model.StreetAddress1,
					StreetAddress2 = model.StreetAddress2,
					City = model.City,
					ZipCode = model.ZipCode,
					State = model.State,
					Email = model.Email,
					TransactionID = transactionID,
					PaymentResource = paymentResource,
					Date = DateTime.Now
				});

				entities.SaveChanges();
			}

			TempData[SuccessfulDonationKey] = true;

			return RedirectToAction("Index");
		}
	}
}