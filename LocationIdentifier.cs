using System;
using System.Collections.Generic;
using System.IO;
using CoreLocation;
using SQLite;

namespace MagpieIOS
{
	public class LocationIdentifier
	{
		private CLLocationManager user;
		private List<CLLocationCoordinate2D> annoList;
		private bool isClose;
		private CLLocation currentLocation;

		public LocationIdentifier()
		{
			user = new CLLocationManager();
			annoList = new List<CLLocationCoordinate2D>();
			isClose = false;
			createAnnotationList();
		}


		//gets all locations and adds them to a list to check against if needed
		private void createAnnotationList() {
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine(documents, "db_sqlite-net.db");
			var db = new SQLiteConnection(dbPath);
			var table = db.Table<MagpieBadge>();

			foreach (MagpieBadge badge in table) {
				CLLocationCoordinate2D loc = new CLLocationCoordinate2D(badge.lat, badge.lon);
				annoList.Add(loc);
			}
		}

		//starts location updataes and checks every time if close to any of the locations
		public void startLocUpdates() {
			user.LocationsUpdated += (sender, e) => {
				foreach (var loc in e.Locations) {
					Console.WriteLine(loc);
					currentLocation = loc;
				}
			};
			user.StartUpdatingLocation();
		}

		//used to stop locations updates if necissary
		public void stopLocUpdates() {
			user.StopUpdatingLocation();
		}

		//returns if given sculpture location close enough to user location.
		public bool checkDistance(CLLocation sculpture, CLLocation current) {
			double meters = current.DistanceFrom(sculpture);
			if (meters <= 3)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

	}
}