using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.Engine.Messages
{
	public interface IMessageLocationFilter
	{
		bool IsMatch(RiderMessageDonation message, IEnumerable<Location> locations);
	}
}
