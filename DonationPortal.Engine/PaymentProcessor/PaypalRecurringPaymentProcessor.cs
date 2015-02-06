using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayPal.Api;

namespace DonationPortal.Engine.PaymentProcessor
{
	public class PaypalRecurringPaymentProcessor : IRecurringPaymentProcessor
	{
		private readonly CreditCardIssuerDetector _issuerDetector;

		public PaypalRecurringPaymentProcessor(CreditCardIssuerDetector issuerDetector)
		{
			_issuerDetector = issuerDetector;
		}

		public RecurringPaymentResult Process(RecurringPaymentRequest request)
		{
			throw new NotImplementedException();
		}
	}
}