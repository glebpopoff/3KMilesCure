namespace DonationPortal.Web.ApiModels.EventDonations
{
	public class RiderDonation
	{
		public decimal DonationAmount { get; set; }
		public float Latitude { get; set; }
		public float Longitude { get; set; }
		public string Message { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string StreetAddress1 { get; set; }
		public string StreetAddress2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string Email { get; set; }
		public string CreditCardNumber { get; set; }
		public int ExpirationMonth { get; set; }
		public int ExpirationYear { get; set; }
		public string CvvNumber { get; set; }
	}
}