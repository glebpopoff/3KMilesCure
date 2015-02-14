using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.Engine.Helpers
{
    public static class DateTimeExtension
    {
        public static string ToAgo(this DateTime date2)
        {
            DateTime date1 = DateTime.Now;
            if (DateTime.Compare(date1, date2) >= 0)
            {
                TimeSpan ts = date1.Subtract(date2);
                if (ts.TotalDays >= 1)
                    return string.Format("{0} days", (int)ts.TotalDays);
                else if (ts.Hours > 2)
                    return string.Format("{0} hours", ts.Hours);
                else if (ts.Hours > 0)
                    return string.Format("{0} hours, {1} minutes",
                           ts.Hours, ts.Minutes);
                else if (ts.Minutes > 5)
                    return string.Format("{0} minutes", ts.Minutes);
                else if (ts.Minutes > 0)
                    return string.Format("{0} mintutes, {1} seconds",
                           ts.Minutes, ts.Seconds);
                else
                    return string.Format("{0} seconds", ts.Seconds);
            }
            else
                return "Not valid";
        }

    }
}
