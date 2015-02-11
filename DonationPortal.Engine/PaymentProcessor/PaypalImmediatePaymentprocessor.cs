using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PayPal.Api;

namespace DonationPortal.Engine.PaymentProcessor
{
	public class PaypalImmediatePaymentProcessor : IImmediatePaymentProcessor
	{
		private static readonly Regex _nonDigitRegex = new Regex(@"[^\d]");

		private readonly CreditCardIssuerDetector _issuerDetector;

		public PaypalImmediatePaymentProcessor(CreditCardIssuerDetector issuerDetector)
		{
			_issuerDetector = issuerDetector;
		}

		public ImmediatePaymentResult Process(ImmediatePaymentRequest request)
		{
			var config = ConfigManager.Instance.GetProperties();

			var accessToken = new OAuthTokenCredential(config).GetAccessToken();

			var apiContext = new APIContext(accessToken)
			{
				Config = config
			};

			var creditCardNumberDigitsOnly = _nonDigitRegex.Replace(request.CreditCardNumber, string.Empty);

			var issuer = _issuerDetector.GetIssuer(creditCardNumberDigitsOnly);

			if (!issuer.HasValue)
			{
				throw new Exception("invalid credit card issuer.");
			}
			var payer = new Payer
			{
				funding_instruments = new List<FundingInstrument>
				{
					new FundingInstrument
					{
						credit_card = new CreditCard
						{
							number = creditCardNumberDigitsOnly,
							expire_month = request.ExpirationMonth,
							expire_year = request.ExpirationYear,
							first_name = request.FirstName,
							last_name = request.LastName,
							cvv2 = request.CvvNumber,
							type = issuer.ToString().ToLower()
						}
					}
				},
				payment_method = "credit_card",
				payer_info = new PayerInfo()
				{
					email = request.Email,
					first_name = request.FirstName,
					last_name = request.LastName,
					billing_address = new Address
					{
						city = request.City,
						line1 = request.StreetAddress1,
						line2 = request.StreetAddress2,
						postal_code = request.ZipCode,
						state = request.State
					}
				}
			};
			
			var payment = new Payment
			{
				intent = "sale",
				payer = payer,
				transactions = new List<Transaction>
				{
					new Transaction
					{
						description = "3000events.com Donation",
						amount = new Amount
						{
							currency = "USD",
							total = request.Amount.ToString("N2")
						}
					}
				}
			};

			var createdPayment = payment.Create(apiContext);

			if (createdPayment.state != "approved")
			{
				throw new Exception("Transaction was not approved.");
			}

			string transactionID;

			if (createdPayment.transactions.Any() && createdPayment.transactions[0].related_resources.Any())
			{
				transactionID = createdPayment.transactions[0].related_resources[0].sale.id;
			}
			else
			{
				transactionID = null;
			}

			return new ImmediatePaymentResult
			{
				PaymentResource = createdPayment.id,
				TransactionID = transactionID
			};
		}
	}
}