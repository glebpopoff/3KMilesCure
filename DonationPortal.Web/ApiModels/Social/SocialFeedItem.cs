namespace DonationPortal.Web.ApiModels.Social
{
    public class SocialFeedItem
	{
		public int ItemID { get; set; }
        public enum SocialType
        {
            Twitter,
            Facebook,
            YouTube   
        }
		public string ImageURL { get; set; }
		public string ImageIcon { get; set; }
		public string UserName { get; set; }
		public string Text { get; set; }
		public string Posted { get; set; }
	}
}