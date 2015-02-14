using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.Engine.PaymentProcessor
{
	public class ImmediatePaymentResult
	{
		public string PaymentResource { get; set; }
		public string TransactionID { get; set; }
	}
}
