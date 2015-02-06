using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DonationPortal.Web.ApiModels.Routes;
using Microsoft.AspNet.SignalR;

namespace DonationPortal.Web.Hubs
{
	public class EventRiderLocationHub : Hub
	{
		public void Hello(string message)
		{
			Clients.All.hello(message);
		}

		public void UpdateLocation(CurrentLocation location)
		{
			Clients.All.updateLocation(location);
		}
	}
}