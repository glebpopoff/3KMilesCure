using System;

namespace DonationPortal.Web.ApiModels.Messages
{
	public class RecentMessage
	{
		public DateTime DateReceived { get; set; }
		public string Username { get; set; }
		public string Message { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
	}
}