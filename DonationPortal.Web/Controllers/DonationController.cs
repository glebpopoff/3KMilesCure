using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationPortal.Engine;
using DonationPortal.Engine.PaymentProcessor;
using DonationPortal.Web.ViewModels.Donation;

namespace DonationPortal.Web.Controllers
{
	public class DonationController : Controller
	{
		private const string SuccessfulDonationKey = "successful_donation";

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
		public ActionResult Index(DonationViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("Index", model);
			}

			string transactionID;

			switch (model.DonationType)
			{
				case NonEventDonationType.OneTimeDonation:
					{
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
					}
					break;
				case NonEventDonationType.MonthlyRecurringDonation:
					{
						var request = new RecurringPaymentRequest
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
							ZipCode = model.ZipCode
						};

						var result = _recurringPaymentProcessor.Process(request);

						transactionID = result.TransactionID;
					}
					break;
				default:
					throw new InvalidOperationException("Unexpected donation type " + model.DonationType);
			}

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
					TransactionID = transactionID
				});

				entities.SaveChanges();
			}

			TempData[SuccessfulDonationKey] = true;

			return RedirectToAction("Index");
		}
	}
}