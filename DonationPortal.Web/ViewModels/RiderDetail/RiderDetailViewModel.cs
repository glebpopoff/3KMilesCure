using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonationPortal.Web.ViewModels.RiderDetail
{
	/// <summary>
	/// todo: really need to extract non-deterministic elements (e.g. DateTime.Now) to test this stuff.
	/// </summary>
	public class RiderDetailViewModel
	{
		public string RiderName { get; set; }
		public string EventName { get; set; }
		public string HeroImageUri { get; set; }
		public string HeroImageText { get; set; }
		public DateTime RiderStart { get; set; }
		public DateTime RiderEnd { get; set; }
		public string PossessiveRiderName { get; set; }
		public decimal DonationTotal { get; set; }
		public decimal DonationGoal { get; set; }
		public int MilesTravelled { get; set; }
		public string MilesGoal { get; set; }
		public string DurationGoal { get; set; }
		public DateTime DonationStart { get; set; }

		public int DonationDaysPast
		{
			get { return (int)(DateTime.Now - DonationStart.Date).TotalDays + 1; }
		}

		/// <summary>
		/// Percentage of the donation goal completed.
		/// </summary>
		public int DonationGoalPercentage
		{
			get
			{
				if (DonationGoal == 0)
				{
					return 0;
				}

				return (int)((DonationTotal / DonationGoal) * 100);
			}
		}

		/// <summary>
		/// Total number of days remaining until the event is over.
		/// </summary>
		public int DaysRemaining
		{
			get
			{
				// if the event finished in the past, show zero.
				if (DateTime.Now.Date > RiderEnd.Date)
				{
					return 0;
				}

				return (int)(RiderEnd.Date - DateTime.Now.Date).TotalDays + 1; // we still have time remaining on the last day.
			}
		}

		/// <summary>
		/// Total number of days in the event.
		/// </summary>
		public int TotalDonationDays
		{
			get { return (int)(RiderEnd.Date - DonationStart.Date).TotalDays + 1; }
		}

		/// <summary>
		/// Friendly text describing the start to end of the event.
		/// </summary>
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

		/// <summary>
		/// Calculates the elapsed time for the event.
		/// If the event has yet to start, this is zero.
		/// If the event is currently underway, this is the time since the event start.
		/// If the event is over, this is the duration of the event.
		/// </summary>
		public TimeSpan TimeElapsed
		{
			get
			{
				// if the event has yet to begin, show all zeros
				if (DateTime.Now < RiderStart)
				{
					return new TimeSpan(0);
				}

				// if the event is over, show the duration of the event
				if (DateTime.Now > RiderEnd)
				{
					return RiderEnd - RiderStart;
				}

				// if the event is currently running, show the difference between now and the start
				return DateTime.Now - RiderStart;
			}
		}

		public TimeSpan EventDuration
		{
			get { return RiderEnd - RiderStart; }
		}

		public bool IsMultipleDays
		{
			get { return EventDuration > new TimeSpan(1, 0, 0, 0); }
		}

		public IHtmlString Teaser { get; set; }
		public string Pronoun { get; set; }
		public IHtmlString RouteDescription { get; set; }
		public IHtmlString DonationDescription { get; set; }
		public IHtmlString ChooseLocationText { get; set; }
		public string ShortEventName { get; set; }
	}
}