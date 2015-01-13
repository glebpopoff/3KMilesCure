using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationPortal.RouteImporter
{
	class Program
	{
		static void Main(string[] args)
		{
			//var import = new CsvRouteImporter(2);
			//import.Import(@"C:\github\3KMilesCure\DonationPortal.RouteImporter\Samples\sebring_short.csv");

			//var import = new CsvRouteImporter(1);
			//import.Import(@"C:\github\3KMilesCure\DonationPortal.RouteImporter\Samples\sebring_racetrack.csv");

			var import = new CsvRouteImporter(3);
			import.Import(@"C:\github\3KMilesCure\DonationPortal.RouteImporter\Samples\sebring_long.csv");
		}
	}
}
