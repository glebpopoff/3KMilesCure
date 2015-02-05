using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayPal.Api;

namespace DonationPortal.Engine.PaymentProcessor
{
	public class PaypalImmediatePaymentProcessor : IImmediatePaymentProcessor
	{
		private readonly string _clientId;
		private readonly string _secret;

		public PaypalImmediatePaymentProcessor(string clientId, string secret)
		{
			_clientId = clientId;
			_secret = secret;
		}

		public ImmediatePaymentResult Process(ImmediatePaymentRequest request)
		{
			var config = new Dictionary<string, string> { { "mode", "sandbox" } };

			var accessToken = new OAuthTokenCredential(_clientId, _secret, config).GetAccessToken();

			var apiContext = new APIContext(accessToken)
			{
				Config = config
			};

			var payer = new Payer
			{
				funding_instruments = new List<FundingInstrument>
				{
					new FundingInstrument
					{
						credit_card = new CreditCard
						{
							number = request.CreditCardNumber,
							expire_month = request.ExpirationMonth,
							expire_year = request.ExpirationYear,
							first_name = request.FirstName,
							last_name = request.LastName,
							cvv2 = request.CvvNumber
						}
					}
				},
				payment_method = "credit_card"
			};
			
			var payment = new Payment
			{
				intent = "sale",
				payer = payer,
				transactions = new List<Transaction>
				{
					new Transaction
					{
						description = "3000 Miles to a Cure Donation",
						amount = new Amount
						{
							currency = "USD",
							total = request.Amount.ToString()
						}
					}
				}
			};

			var createdPayment = payment.Create(apiContext);
			
			return new ImmediatePaymentResult
			{
				TransactionID = createdPayment.id
			};
		}
	}
}