using System.Linq;
using DotSpatial.Positioning;

namespace DonationPortal.Engine.Rider
{
	public class EventRiderLocationProvider
	{
		public Position? GetLocation(int eventRiderID)
		{
			// just grab the latest location they have visited.
			using (var entities = new DonationPortalEntities())
			{
				var latestLocation = entities.LocationVisits
					.Where(v => v.EventRiderID == eventRiderID)
					.OrderByDescending(v => v.DateVisited)
					.FirstOrDefault();

				if (latestLocation == null)
				{
					return null;
				}

				return new Position(new Latitude(latestLocation.Latitude), new Longitude(latestLocation.Longtitude));
			}
		}

		public Distance GetTotalDistance(int eventRiderID)
		{
			// sort by date visited, then calculate the distance between every pair sequentially.  sum it up.
			using (var entities = new DonationPortalEntities())
			{
				// only look at locations visited during the race.  not before or after.
				var visits = entities.LocationVisits
					.Where(v => v.EventRiderID == eventRiderID && v.DateVisited > v.EventRider.Start && v.DateVisited < v.EventRider.End)
					.OrderBy(v => v.DateVisited).ToList();

				if (visits.Count < 2)
				{
					return new Distance();
				}

				var previous = visits[0];
				var sum = new Distance();

				for (var i = 1; i < visits.Count; i++)
				{
					var next = visits[i];

					sum = sum.Add(
						new Position(new Latitude(previous.Latitude), new Longitude(previous.Longtitude)).DistanceTo(
							new Position(new Latitude(next.Latitude), new Longitude(next.Longtitude))));

					previous = next;
				}

				return sum;
			}
		}
	}
}
