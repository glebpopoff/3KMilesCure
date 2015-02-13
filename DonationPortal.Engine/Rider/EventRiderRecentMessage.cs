using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.Engine.Rider
{
	public class EventRiderRecentMessage
	{
		public int EventRiderID { get; set; }
		public string Text { get; set; }
		public string Sender { get; set; }
		public DateTime DateReceived { get; set; }
	}
}
