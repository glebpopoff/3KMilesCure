using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DonationPortal.Web.Attributes
{
	public class ConditionalRequireHttpsAttribute : RequireHttpsAttribute
	{
		protected override void HandleNonHttpsRequest(AuthorizationContext filterContext)
		{
			bool requireSsl;
			bool.TryParse(ConfigurationManager.AppSettings["RequireSSL"], out requireSsl);

			if (!requireSsl)
			{
				return;
			}

			base.HandleNonHttpsRequest(filterContext);
		}
	}
}