using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.Engine.PaymentProcessor
{
	public class MockRecurringPaymentProcessor : IRecurringPaymentProcessor
	{
		public RecurringPaymentResult Process(RecurringPaymentRequest request)
		{
			return new RecurringPaymentResult
			{
				TransactionID = "123"
			};
		}
	}
}
