using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonationPortal.Web.ApiModels.Message
{
	public class DonationMessage
	{
		public int ID { get; set; }
		public string Sender { get; set; }
		public string Message { get; set; }
		public float Latitude { get; set; }
		public float Longitude { get; set; }
	}
}