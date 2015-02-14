using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonationPortal.Web.ViewModels
{
	public class TimerViewModel
	{
		public TimerViewModel(string durationGoal, DateTime riderEnd, DateTime riderStart)
		{
			DurationGoal = durationGoal;
			RiderEnd = riderEnd;
			RiderStart = riderStart;
		}

		public DateTime RiderStart { get; private set; }
		public DateTime RiderEnd { get; private set; }
		public string DurationGoal { get; private set; }

		public TimeSpan EventDuration
		{
			get { return RiderEnd - RiderStart; }
		}

		public bool IsMultipleDays
		{
			get
			{
				return EventDuration > new TimeSpan(1, 0, 0, 0);
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

		public string ElapsedDays
		{
			get { return String.Format("{0:00}", TimeElapsed.Days); }
		}

		public string ElapsedHours
		{
			get
			{
				return String.Format("{0:00}", TimeElapsed.Hours);
			}
		}

		public string ElapsedMinutes
		{
			get
			{
				return String.Format("{0:00}", TimeElapsed.Minutes);
			}
		}

		public bool HasDurationGoal
		{
			get
			{
				return !string.IsNullOrWhiteSpace(DurationGoal);
			}
		}
	}
}