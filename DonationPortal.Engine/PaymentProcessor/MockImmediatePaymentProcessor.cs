using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.Engine.PaymentProcessor
{
	public class MockImmediatePaymentProcessor : IImmediatePaymentProcessor
	{
		public ImmediatePaymentResult Process(ImmediatePaymentRequest request)
		{
			return new ImmediatePaymentResult
			{
				TransactionID = "1234567890"
			};
		}
	}
}
