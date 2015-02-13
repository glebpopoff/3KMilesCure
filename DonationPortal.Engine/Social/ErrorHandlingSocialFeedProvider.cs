using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace DonationPortal.Engine.Social
{
	public class ErrorHandlingSocialFeedProvider : ISocialFeedProvider
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ErrorHandlingSocialFeedProvider));

		private readonly ISocialFeedProvider _socialFeedProvider;

		public ErrorHandlingSocialFeedProvider(ISocialFeedProvider socialFeedProvider)
		{
			_socialFeedProvider = socialFeedProvider;
		}

		public IEnumerable<SocialFeedItem> GetItems(int eventRiderID, int count = 100)
		{
			try
			{
				return _socialFeedProvider.GetItems(eventRiderID, count);
			}
			catch (Exception ex)
			{
				_log.Error(string.Format("Error retrieving social feed for rider {0}.", eventRiderID), ex);

				return new SocialFeedItem[0];
			}
		}
	}
}
