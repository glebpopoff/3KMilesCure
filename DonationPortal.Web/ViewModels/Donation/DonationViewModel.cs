using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DonationPortal.Web.Attributes;

namespace DonationPortal.Web.ViewModels.Donation
{
	public class DonationViewModel
	{
		[Required]
		[AllowedValues(typeof(string), "25", "50", "75", "100", "other")]
		public string SelectedDonationAmount { get; set; }

		// gotta think about this one...
		// [RequiredIf("SelectedDonationAmount", "other")]
		public decimal? OtherDonationAmount { get; set; }

		[Required]
		[EnumDataType(typeof(NonEventDonationType))]
		public NonEventDonationType DonationType { get; set; }

		[Required]
		[StringLength(255, MinimumLength = 1)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(255, MinimumLength = 1)]
		public string LastName { get; set; }

		[Required]
		[StringLength(255, MinimumLength = 1)]
		public string StreetAddress1 { get; set; }

		[Required]
		[StringLength(255, MinimumLength = 1)]
		public string StreetAddress2 { get; set; }

		[Required]
		[StringLength(255, MinimumLength = 1)]
		public string City { get; set; }

		[Required]
		[StringLength(2, MinimumLength = 2)]
		public string State { get; set; }

		[Required]
		[StringLength(10, MinimumLength = 5)]
		public string ZipCode { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		// basic format validation?
		public string CreditCardNumber { get; set; }

		[Required]
		[Range(1, 12)]
		public int ExpirationMonth { get; set; }

		[Required]
		// could do some validation here?
		// really should make sure it is one of the dropdown values
		public int ExpirationYear { get; set; }

		[Required]
		[StringLength(3, MinimumLength = 3)] // is this true?
		public string CvvNumber { get; set; }

		public bool HasSuccessfulDonation { get; set; }

		public decimal DonationAmount
		{
			get
			{
				if (string.Equals(SelectedDonationAmount, "other"))
				{
					// let's not throw here, just return zero if other validation has failed anyway.
					return OtherDonationAmount.HasValue ? OtherDonationAmount.Value : 0;
				}

				decimal amount;

				// we're okay with returning zero if validation failed,
				// so we don't really care whether or not this operation succeeds.
				decimal.TryParse(SelectedDonationAmount, out amount);

				return amount;
			}
		}

		public IEnumerable<SelectListItem> States
		{
			get
			{
				return new[]
				{
					new SelectListItem{ Value = "AL", Text = "Alabama" },
					new SelectListItem{ Value = "AK", Text = "Alaska" },
					new SelectListItem{ Value = "AZ", Text = "Arizona" },
					new SelectListItem{ Value = "AR", Text = "Arkansas" },
					new SelectListItem{ Value = "CA", Text = "California" },
					new SelectListItem{ Value = "CO", Text = "Colorado" },
					new SelectListItem{ Value = "CT", Text = "Connecticut" },
					new SelectListItem{ Value = "DE", Text = "Delaware" },
					new SelectListItem{ Value = "DC", Text = "Dist of Columbia" },
					new SelectListItem{ Value = "FL", Text = "Florida" },
					new SelectListItem{ Value = "GA", Text = "Georgia" },
					new SelectListItem{ Value = "HI", Text = "Hawaii" },
					new SelectListItem{ Value = "ID", Text = "Idaho" },
					new SelectListItem{ Value = "IL", Text = "Illinois" },
					new SelectListItem{ Value = "IN", Text = "Indiana" },
					new SelectListItem{ Value = "IA", Text = "Iowa" },
					new SelectListItem{ Value = "KS", Text = "Kansas" },
					new SelectListItem{ Value = "KY", Text = "Kentucky" },
					new SelectListItem{ Value = "LA", Text = "Louisiana" },
					new SelectListItem{ Value = "ME", Text = "Maine" },
					new SelectListItem{ Value = "MD", Text = "Maryland" },
					new SelectListItem{ Value = "MA", Text = "Massachusetts" },
					new SelectListItem{ Value = "MI", Text = "Michigan" },
					new SelectListItem{ Value = "MN", Text = "Minnesota" },
					new SelectListItem{ Value = "MS", Text = "Mississippi" },
					new SelectListItem{ Value = "MO", Text = "Missouri" },
					new SelectListItem{ Value = "MT", Text = "Montana" },
					new SelectListItem{ Value = "NE", Text = "Nebraska" },
					new SelectListItem{ Value = "NV", Text = "Nevada" },
					new SelectListItem{ Value = "NH", Text = "New Hampshire" },
					new SelectListItem{ Value = "NJ", Text = "New Jersey" },
					new SelectListItem{ Value = "NM", Text = "New Mexico" },
					new SelectListItem{ Value = "NY", Text = "New York" },
					new SelectListItem{ Value = "NC", Text = "North Carolina" },
					new SelectListItem{ Value = "ND", Text = "North Dakota" },
					new SelectListItem{ Value = "OH", Text = "Ohio" },
					new SelectListItem{ Value = "OK", Text = "Oklahoma" },
					new SelectListItem{ Value = "OR", Text = "Oregon" },
					new SelectListItem{ Value = "PA", Text = "Pennsylvania" },
					new SelectListItem{ Value = "RI", Text = "Rhode Island" },
					new SelectListItem{ Value = "SC", Text = "South Carolina" },
					new SelectListItem{ Value = "SD", Text = "South Dakota" },
					new SelectListItem{ Value = "TN", Text = "Tennessee" },
					new SelectListItem{ Value = "TX", Text = "Texas" },
					new SelectListItem{ Value = "UT", Text = "Utah" },
					new SelectListItem{ Value = "VT", Text = "Vermont" },
					new SelectListItem{ Value = "VA", Text = "Virginia" },
					new SelectListItem{ Value = "WA", Text = "Washington" },
					new SelectListItem{ Value = "WV", Text = "West Virginia" },
					new SelectListItem{ Value = "WI", Text = "Wisconsin" },
					new SelectListItem{ Value = "WY", Text = "Wyoming" },
				};
			}
		}

		public IEnumerable<SelectListItem> ExpirationMonths
		{
			get
			{
				return Enumerable.Range(1, 12).Select(m => m.ToString("00")).Select(m => new SelectListItem { Text = m, Value = m });
			}
		}

		public IEnumerable<SelectListItem> ExpirationYears
		{
			get
			{
				return Enumerable.Range(DateTime.Now.Year, 10).Select(m => m.ToString("0000")).Select(m => new SelectListItem { Text = m, Value = m });
			}
		}
	}
}