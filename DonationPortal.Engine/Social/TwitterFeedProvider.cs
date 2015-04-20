using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using LinqToTwitter;
using Tweetinvi.Core.Interfaces;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Core.Interfaces.Models.Parameters;
using Tweetinvi.Core.Interfaces.oAuth;

namespace DonationPortal.Engine.Social
{
	public class TwitterFeedProvider : ISocialFeedProvider
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof (TwitterFeedProvider));
		
        public TwitterFeedProvider(string oAuthToken, string oAuthTokenSecret, string consumerKey, string consumerSecret)
        {
            TwitterCredentials.SetCredentials(
                oAuthToken,
                oAuthTokenSecret,
                consumerKey,
                consumerSecret
            );
        }

        public IEnumerable<SocialFeedItem> GetItems(int eventRiderID, int count = 50)
        {
            using (var entities = new DonationPortalEntities())
            {
                var rider = entities.EventRiders.SingleOrDefault(r => r.EventRiderID == eventRiderID);

                if (rider == null)
                {
                    return new SocialFeedItem[0];
                }
     
                var timelineTweets = new List<ITweet>();
                var hashtagTweets = new List<ITweet>();

                foreach (var username in rider.SocialAccounts.Where(a => a.SocialType == "Twitter").Select(a => a.Username))
                {
                    var user = Tweetinvi.User.GetUserFromScreenName(username);
                    timelineTweets.AddRange(user.GetUserTimeline(count)); //GetUserTimeline defaults to 40 if left out
                }

                foreach (var hashtag in rider.SocialAccounts.Where(a => a.SocialType == "Twitter")
                                                .SelectMany(a => a.TwitterHashTags.Select(h => h.HashTag))
                                                .Distinct())
                {
                    var searchParameter = Tweetinvi.Search.GenerateTweetSearchParameter("#" + hashtag);
                    searchParameter.TweetSearchFilter = TweetSearchFilter.OriginalTweetsOnly;

                    var tweets = Tweetinvi.Search.SearchTweets(searchParameter);
                    hashtagTweets.AddRange(tweets);
                }

                
                timelineTweets.Intersect(hashtagTweets);
                    
                if (timelineTweets.Count == 0)
                {
                    return new SocialFeedItem[0];
                }

                //sort all the tweets we got from the usernames and hashtags.
                return timelineTweets.Distinct().OrderByDescending(t => t.CreatedAt).Take(count).Select(TweetItem);
                    
            }
        }

        public static Tweetinvi.Core.Interfaces.IUser GetUserFromScreenName(string userName)
        {
            return Tweetinvi.User.UserFactory.GetUserFromScreenName(userName);
        }

        private static SocialFeedItem TweetItem(ITweet tweet)
        {
            SocialFeedItem retweetItem = null;
            if (tweet.Retweeted)
            {
                //this is a retweet
                retweetItem = new SocialFeedItem();
                retweetItem.ImageURL = tweet.RetweetedTweet.Creator.ProfileImageUrl;
                retweetItem.UserName = tweet.RetweetedTweet.Creator.UserIdentifier.ScreenName;
                retweetItem.Name = tweet.RetweetedTweet.Creator.Name;
            }
            string plainText = tweet.Text;
            //edit links
            foreach (IUrlEntity uEntity in tweet.Entities.Urls)
            {
                string oldURLText = uEntity.URL;
                string newURLText = String.Format("<a target='_blank' href='{0}'>{1}</a>", oldURLText, uEntity.DisplayedURL);
                plainText = plainText.Replace(oldURLText, newURLText);

                //let's also update the image url if applicable.
                if (tweet.Entities.Medias != null && tweet.Entities.Medias.Count > 0)
                {
                    tweet.Entities.Medias[0].MediaURL = oldURLText;
                }
            }
            foreach (IHashtagEntity htEntity in tweet.Entities.Hashtags)
            {
                string linkedHashTag = String.Format("<a target='_blank' href='https://twitter.com/hashtag/{0}?src=hash'>#{0}</a>", htEntity.Text);
                plainText = plainText.Replace("#" + htEntity.Text, linkedHashTag);
            }
            foreach (IUserMentionEntity user in tweet.Entities.UserMentions)
            {
                string linkedUser = String.Format("<a target='_blank' href='https://twitter.com/{0}'>@{0}</a>", user.ScreenName);
                plainText = plainText.Replace("@" + user.ScreenName, linkedUser);
            }

            return new SocialFeedItem(Convert.ToUInt64(tweet.Id), plainText, tweet.Creator.ProfileImageUrl, tweet.Media, tweet.Creator.UserIdentifier.ScreenName, tweet.Creator.Name, tweet.CreatedAt.ToLocalTime(), retweetItem);
        }
	}
}
