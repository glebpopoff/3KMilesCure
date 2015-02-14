using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DonationPortal.Engine.Rider
{
	public class EventRiderMessageProvider
	{
		public IEnumerable<RecentMessage> GetMessages(int riderID, int count)
		{
			using (var entities = new DonationPortalEntities())
			{
				return entities.RecentMessages
					.Include(m => m.RiderMessageDonation)
					.Where(m => m.RiderMessageDonation.EventRider.EventRiderID == riderID)
					.OrderByDescending(d => d.DateReceived)
					.Take(count)
					.ToList();
			}
		}
	}
}