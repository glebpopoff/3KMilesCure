using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using DonationPortal.Web.Attributes;

namespace DonationPortal.Web.ApiModels.EventDonations
{
	public class RiderDonation
	{
		[Required]
		public float Latitude { get; set; }

		[Required]
		public float Longitude { get; set; }

		[Required]
		[StringLength(120, MinimumLength = 1)]
		public string Message { get; set; }

		[Required]
		[DisplayName("Donation Amount")]
		[Min(1)]
		public decimal DonationAmount { get; set; }

		[Required]
		[StringLength(255, MinimumLength = 1)]
		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[Required]
		[StringLength(255, MinimumLength = 1)]
		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[Required]
		[StringLength(255, MinimumLength = 1)]
		[DisplayName("Street Address 1")]
		public string StreetAddress1 { get; set; }

		[DisplayName("Street Address 2")]
		[StringLength(255, MinimumLength = 1)]
		public string StreetAddress2 { get; set; }

		[Required]
		[StringLength(255, MinimumLength = 1)]
		public string City { get; set; }

		[Required]
		[StringLength(2, MinimumLength = 2)]
		public string State { get; set; }

		[Required]
		[DisplayName("Zip Code")]
		[StringLength(10, MinimumLength = 5)]
		public string ZipCode { get; set; }

		[Required]
		[EmailAddress]
		[StringLength(255, MinimumLength = 1)]
		public string Email { get; set; }

		[Required]
		[System.ComponentModel.DataAnnotations.CreditCard]
		[DisplayName("Credit Card Number")]
		public string CreditCardNumber { get; set; }

		[Required]
		[DisplayName("Expiration Month")]
		[Range(1, 12)]
		public int ExpirationMonth { get; set; }

		[Required]
		[DisplayName("Expiration Year")]
		public int ExpirationYear { get; set; }

		[Required]
		[DisplayName("CVV Number")]
		public string CvvNumber { get; set; }
	}
}