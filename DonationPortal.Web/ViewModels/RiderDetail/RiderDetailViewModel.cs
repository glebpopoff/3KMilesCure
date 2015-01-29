using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonationPortal.Web.ViewModels.RiderDetail
{
	public class RiderDetailViewModel
	{
		public string RiderName { get; set; }
		public string EventName { get; set; }
		public string HeroImageUri { get; set; }
		public string HeroImageText { get; set; }
		public DateTime RiderStart { get; set; }
		public DateTime RiderEnd { get; set; }
		public string PossessiveRiderName { get; set; }

		public string DateRange
		{
			get
			{
				if (RiderStart.Date.Equals(RiderEnd.Date))
				{
					// starts and ends all on one day.
					return RiderStart.ToString("MMM d, yyyy");
				}

				if (RiderStart.Year.Equals(RiderEnd.Year))
				{
					if (RiderStart.Month.Equals(RiderEnd.Month))
					{
						// same month, but different days

						return string.Format("{0} {1}-{2}, {3}", RiderStart.ToString("MMM"), RiderStart.Day, RiderEnd.Day, RiderStart.Year);
					}

					// we're dealing with the same year, but different months

					return string.Format("{0}-{1}, {2}", RiderStart.ToString("MMM d"), RiderEnd.ToString("MMM d"), RiderStart.Year);
				}

				// we're dealing with different years
				return string.Format("{0}-{1}", RiderStart.ToString("MMM d, yyyy"), RiderEnd.ToString("MMM d, yyyy"));
			}
		}
	}
}