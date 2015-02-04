using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.Engine.PaymentProcessor
{
	public class RecurringPaymentRequest
	{
		public decimal Amount { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string StreetAddress1 { get; set; }
		public string StreetAddress2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string Email { get; set; }
		public int ExpirationMonth { get; set; }
		public int ExpirationYear { get; set; }
		public string CvvNumber { get; set; }
		// recur type?
		// start date?
	}
}
