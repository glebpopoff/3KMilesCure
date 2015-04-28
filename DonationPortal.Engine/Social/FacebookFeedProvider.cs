using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.Engine.Social
{
    public class FacebookFeedProvider : ISocialFeedProvider
    {
        private readonly string _pageID;
        private readonly string _accessToken;

        public FacebookFeedProvider(string pageID, string accessToken)
        {
            if (pageID == null)
                throw new ArgumentNullException("pageID");

            if (accessToken == null)
                throw new ArgumentNullException("accessToken");

            if (string.IsNullOrWhiteSpace(pageID))
                throw new ArgumentException("pageID cannot be empty.");

            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentException("accessToken cannot be empty.");

            this._pageID = pageID;
            this._accessToken = accessToken;
        }

        public IEnumerable<SocialFeedItem> GetItems(int eventRiderId, int count = 50)
        {
            var client = new FacebookClient(this._accessToken);

            dynamic response = client.Get(this._pageID + "/posts");

            if (!response.ContainsKey("data") && response.data is JsonArray)
                return null;

            var posts = ((JsonArray)(response.data))
                .Select(p => new SocialFeedItem(p))
                .Where(p => p.Text != null)
                .OrderByDescending(p => p.Posted).ToList();

            return posts;
            
        }
    }
}
