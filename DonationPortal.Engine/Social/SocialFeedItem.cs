using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace DonationPortal.Engine.Social
{
    public class SocialFeedItem
	{
        public enum SocialType
        {
            Twitter,
            Facebook,
            YouTube   
        }

		public ulong ItemID { get; set; }
        public SocialFeedItem ReTweetItem { get; set; }
        public string ImageURL { get; set; }
        public Photo MediaPhoto { get; set; }
        public string ImageIcon { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
		public string Text { get; set; }
		public DateTime Posted { get; set; }
        public string Annotation { get; set; }
        public SocialType Type { get; set; }

       
        public SocialFeedItem(ulong statusID,
                                string text,
                                string url,
                                List<IMediaEntity> media,
                                string username,
                                string name,
                                DateTime posted,
                                SocialFeedItem retweetItem)
        {
            Text = text;
            ImageURL = url;
            if (media != null && media.Count > 0)
            {
                Photo mp = new Photo();
                mp.Src = media[0].MediaURL;
                mp.Href = media[0].MediaURLHttps;
                MediaPhoto = mp;
            }
            UserName = username;
            Name = name;
            Posted = posted;
            ItemID = statusID;
            ReTweetItem = retweetItem;
            Type = SocialType.Twitter;
        }

        public SocialFeedItem(dynamic post)
        {
            Text = post.message;

            DateTime tempDateTime = new DateTime();

            if (post.ContainsKey("updated_time"))
            {
                DateTime.TryParse(post.updated_time, out tempDateTime);
            }


            ImageURL = post.link;
            ImageIcon = post.picture;
            UserName = post.from.name;
            Name = post.name;
            Posted = tempDateTime;
            Type = SocialType.Facebook;
        }
        
        public SocialFeedItem()
        {
            // TODO: Complete member initialization
        }
	}
    public class Photo
    {
        public String Src { get; set; }
        public String Href { get; set; }
    }
}