using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Positioning;

namespace DonationPortal.Engine.Messages
{
	public class DistanceMessageLocationFilter : IMessageLocationFilter
	{
		private readonly Distance _maximumDistance;

		public DistanceMessageLocationFilter(Distance maximumDistance)
		{
			this._maximumDistance = maximumDistance;
		}

		public bool IsMatch(RiderMessageDonation message, IEnumerable<Location> locations)
		{
			var clientLocations = locations.Select(l => new Position(
				   new Latitude(l.Latitude),
				   new Longitude(l.Longitude))).ToList();

			var messageTarget = new Position(
				new Latitude(message.Latitude),
				new Longitude(message.Longitude));

			var index = 0;
			var match = false;
			while (index < clientLocations.Count && !match)
			{
				var clientLocation = clientLocations[index];

				if (clientLocation.DistanceTo(messageTarget) < _maximumDistance)
				{
					match = true;
				}

				index++;
			}

			return match;
		}
	}
}
