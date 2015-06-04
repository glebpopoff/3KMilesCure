namespace DonationPortal.Web.ApiModels.Messages
{
	public class DonationMessage
	{
		public int ID { get; set; }
		public string Sender { get; set; }
		public string Message { get; set; }
		public float Latitude { get; set; }
		public float Longitude { get; set; }
        public decimal Amount { get; set; }
	}
}