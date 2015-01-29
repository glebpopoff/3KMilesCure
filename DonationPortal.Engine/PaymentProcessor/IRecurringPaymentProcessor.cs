using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.Engine.PaymentProcessor
{
	public interface IRecurringPaymentProcessor
	{
		RecurringPaymentResult Process(RecurringPaymentRequest request);
	}
}
