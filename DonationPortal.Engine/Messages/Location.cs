using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.Engine.Messages
{
	public class Location
	{
		public float Latitude { get; set; }
		public float Longitude { get; set; }
        public DateTime Date { get; set; }
		public override string ToString()
		{
			return string.Format("{0} {1}", Latitude, Longitude);
		}
	}
}
