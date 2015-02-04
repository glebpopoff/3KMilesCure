using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.Engine
{
	public class AppSettingFeaturedEventProvider
	{
		public int FeaturedEventID
		{
			get
			{
				// i'm okay with this throwing exceptions, the stack trace will be obvious as to the configuration issue.
				return int.Parse(ConfigurationManager.AppSettings["FeaturedEventID"]);
			}
		}
	}
}
