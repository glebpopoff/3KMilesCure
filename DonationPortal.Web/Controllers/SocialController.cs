using DonationPortal.Web.ApiModels.Social;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToTwitter;
using System.Threading.Tasks;
using System.Configuration;

namespace DonationPortal.Web.Controllers
{
    public class SocialController : Controller
    {
        private static List<SocialFeedItem> _items;//public list of tweets to display

        // Only for requesting oAuthToken
        public ActionResult Authorize()
        {
            if (!new SessionStateCredentialStore().HasAllCredentials())
                return RedirectToAction("Begin", "OAuth");
            else
                return RedirectToAction("Index");
        }

        public async Task<ActionResult> Index()
        {
            //clear the list of Twitter items
            _items = new List<SocialFeedItem>();

            var auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["TwitterconsumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["TwitterconsumerSecret"],
                    OAuthToken = ConfigurationManager.AppSettings["TwitterOAuthToken"],
                    OAuthTokenSecret = ConfigurationManager.AppSettings["TwitterOAuthTokenSecret"]
                }
            };

            using (var twitterCtx = new TwitterContext(auth))
            {
                //check to make sure authorized successfully
                bool authorized = false;
                try
                {
                    var verifyResponse =
                        await
                            (from acct in twitterCtx.Account
                             where acct.Type == AccountType.VerifyCredentials
                             select acct)
                            .SingleOrDefaultAsync();

                    if (verifyResponse != null && verifyResponse.User != null)
                    {
                        authorized = true;
                    }
                }
                catch (TwitterQueryException tqe)
                {
                    //Console.WriteLine(tqe.Message);
                }

                //if not authorized properly, return no results.
                if (!authorized)
                {
                    return View(_items);
                }

                //var tweets =
                //    await
                //   (from tweet in twitterCtx.Status
                //    where tweet.Type == StatusType.User && tweet.User.ScreenName == "3000toacure"
                //    select new SocialFeedItem
                //    {
                //        ImageURL = tweet.User.ProfileImageUrl,
                //        UserName = tweet.User.ScreenNameResponse,
                //        Name = tweet.User.Name,
                //        Text = tweet.Text,
                //        Annotation = tweet.Annotation.ToString(),
                //        Posted = DateTime.Now.Subtract(tweet.CreatedAt.ToLocalTime()).Hours.ToString("#.#"),
                //        ItemID = (long)tweet.ID
                //    })
                //   .ToListAsync();

                string strHashTags = "";
                string strUsers = "";
                //get list of hash tags and users from web.config
                foreach (string s in ConfigurationManager.AppSettings["TwitterHashTags"].Split(';'))
                {
                    if (strHashTags == "")
                    {
                        strHashTags = String.Format("#{0}", s);
                    }
                    else
                    {
                        strHashTags += String.Format(" OR #{0}", s);
                    }
                } 
                foreach (string s in ConfigurationManager.AppSettings["TwitterUsers"].Split(';'))
                {
                    if (strUsers == "")
                    {
                        strUsers = String.Format("from:{0}", s);
                    }
                    else
                    {
                        strUsers += String.Format(" OR from:{0}", s);
                    }
                }

                var searchResponse =
                  await
                  (from search in twitterCtx.Search
                   where search.Type == SearchType.Search &&
                         search.Query == String.Format("{0} {1}", strHashTags, strUsers) &&
                         search.IncludeEntities == true &&
                         search.ResultType == ResultType.Recent &&
                         search.Count == 100
                   select search)
                  .SingleOrDefaultAsync();

                if (searchResponse != null && searchResponse.Statuses != null)
                    searchResponse.Statuses.ForEach(TweetItem);

                return View(_items);

            }
        }

        private static void TweetItem(Status tweet)
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
            foreach(UrlEntity uEntity in tweet.Entities.UrlEntities){
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
            _items.Add(new SocialFeedItem(tweet.StatusID, plainText, tweet.User.ProfileImageUrl, tweet.Entities.MediaEntities, tweet.User.ScreenNameResponse, tweet.User.Name, tweet.CreatedAt.ToLocalTime(), retweetItem));
        }
    }
}