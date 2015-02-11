using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace DonationPortal.Engine.Social
{
	public class FeedProvider
	{
		private readonly ICredentialStore _credentialStore;

		public FeedProvider(ICredentialStore credentialStore)
		{
			_credentialStore = credentialStore;
		}

		public async Task<IEnumerable<SocialFeedItem>> GetItems(int eventRiderID, int count = 100)
		{
			using (var entities = new DonationPortalEntities())
			{
				var rider = entities.EventRiders.SingleOrDefault(r => r.EventRiderID == eventRiderID);

				if (rider == null)
				{
					return new SocialFeedItem[0];
				}

				using (var twitterCtx = new TwitterContext(new SingleUserAuthorizer()
				{
					CredentialStore = _credentialStore
				}))
				{
					string strHashTags = "";
					string strUsers = "";

					foreach (var username in rider.SocialAccounts.Where(a => a.SocialType == "Twitter").Select(a => a.Username))
					{
						if (strUsers == "")
						{
							strUsers = String.Format("from:{0}", username);
						}
						else
						{
							strUsers += String.Format(" OR from:{0}", username);
						}
					}

					// for now we are not limiting results to hashtags for specific users.  search for all hashtags for all users.
					foreach (
						var hashtag in
							rider.SocialAccounts.Where(a => a.SocialType == "Twitter")
								.SelectMany(a => a.TwitterHashTags.Select(h => h.HashTag))
								.Distinct())
					{
						if (strHashTags == "")
						{
							strHashTags = String.Format("#{0}", hashtag);
						}
						else
						{
							strHashTags += String.Format(" OR #{0}", hashtag);
						}
					}

					var query = String.Format("{0} {1}", strHashTags, strUsers);

					if (string.IsNullOrWhiteSpace(query))
					{
						return new SocialFeedItem[0];
					}

					var searchResponse = await
						(from search in twitterCtx.Search
						 where search.Type == SearchType.Search &&
							   search.Query == query &&
							   search.IncludeEntities == true &&
							   search.ResultType == ResultType.Recent &&
							   search.Count == count
						 select search)
							.SingleOrDefaultAsync();

					if (searchResponse != null && searchResponse.Statuses != null)
					{
						return searchResponse.Statuses.Select(TweetItem);
					}

					return new SocialFeedItem[0];
				}
			}
		}

		private static SocialFeedItem TweetItem(Status tweet)
		{
			SocialFeedItem retweetItem = null;
			if (tweet.RetweetedStatus.User != null)
			{
				//this is a retweet
				retweetItem = new SocialFeedItem();
				retweetItem.ImageURL = tweet.RetweetedStatus.User.ProfileImageUrl;
				retweetItem.UserName = tweet.RetweetedStatus.User.ScreenNameResponse;
				retweetItem.Name = tweet.RetweetedStatus.User.Name;
			}
			string plainText = tweet.Text;
			//edit links
			foreach (UrlEntity uEntity in tweet.Entities.UrlEntities)
			{
				string oldURLText = uEntity.Url;
				string newURLText = String.Format("<a target='_blank' href='{0}'>{1}</a>", oldURLText, uEntity.DisplayUrl);
				plainText = plainText.Replace(oldURLText, newURLText);

				//let's also update the image url if applicable.
				tweet.Entities.MediaEntities[0].MediaUrlHttps = oldURLText;
			}
			foreach (HashTagEntity htEntity in tweet.Entities.HashTagEntities)
			{
				string linkedHashTag = String.Format("<a target='_blank' href='https://twitter.com/hashtag/{0}?src=hash'>#{0}</a>", htEntity.Tag);
				plainText = plainText.Replace("#" + htEntity.Tag, linkedHashTag);
			}
			foreach (UserMentionEntity user in tweet.Entities.UserMentionEntities)
			{
				string linkedUser = String.Format("<a target='_blank' href='https://twitter.com/{0}'>@{0}</a>", user.ScreenName);
				plainText = plainText.Replace("@" + user.ScreenName, linkedUser);
			}

			return new SocialFeedItem(tweet.StatusID, plainText, tweet.User.ProfileImageUrl, tweet.Entities.MediaEntities, tweet.User.ScreenNameResponse, tweet.User.Name, tweet.CreatedAt.ToLocalTime(), retweetItem);
		}
	}
}
