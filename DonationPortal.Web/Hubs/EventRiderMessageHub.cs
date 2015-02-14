using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DonationPortal.Engine;
using DonationPortal.Engine.Rider;
using DonationPortal.Web.ApiModels.Routes;
using Microsoft.AspNet.SignalR;

namespace DonationPortal.Web.Hubs
{
	public class EventRiderMessageHub : Hub
	{
		public void AddRecentMessage(EventRiderRecentMessage message)
		{
			Clients.All.addMessage(message);
		}
	}
}