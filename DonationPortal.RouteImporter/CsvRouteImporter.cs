using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace DonationPortal.RouteImporter
{
	class CsvRouteImporter
	{
		private readonly int _routeID;

		public CsvRouteImporter(int routeID)
		{
			_routeID = routeID;
		}

		public void Import(string filename)
		{
			using (var fileStream = File.Open(filename, FileMode.Open, FileAccess.Read))
			using (var streamReader = new StreamReader(fileStream))
			using (var csvReader = new CsvReader(streamReader))
			using (var entities = new DonationPortalEntities())
			{
				var i = 0;

				entities.Routes.Single(r => r.RouteID == _routeID).RouteVertexes.Clear();

				while (csvReader.Read())
				{
					var latitude = csvReader.GetField<float>("Latitude");
					var longitude = csvReader.GetField<float>("Longitude");
					var order = i++;

					entities.RouteVertexes.Add(new RouteVertex
					{
						Latitude = latitude,
						Longitude = longitude,
						Order = order,
						RouteID = _routeID
					});
				}

				entities.SaveChanges();
			}
		}
	}
}
