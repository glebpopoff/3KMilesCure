using System.Collections.Generic;

namespace DonationPortal.Engine.Social
{
	public interface ISocialFeedProvider
    {
        IEnumerable<SocialFeedItem> GetItems(int eventRiderID, int count = 100);
	}
}