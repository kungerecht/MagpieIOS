using System;
using System.IO;
using CoreLocation;
using SQLite;

namespace MagpieIOS
{
	public class ClaimBadge
	{

		string dbName;
		string documents;
		string dbPath;

		public ClaimBadge()
		{
			this.dbName = "db_sqlite-net.db";
			this.documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			this.dbPath = Path.Combine(documents, dbName);
		}

		private bool doesExist()
		{
			
			if (File.Exists(dbPath))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public void claimLocalBadge(CLLocationCoordinate2D badge)
		{ 
			var db = new SQLiteConnection(dbPath);

			//get badge id for badge with lat and lon
			double badgeLat = badge.Latitude;
			string bquery = "SELECT bid FROM MagpieBadge WHERE lat = " + badgeLat;
			var badgeID = db.Query<MagpieBadge>(bquery);

			//execute update on
			String query = "UPDATE MagpieUser SET isClaimed = '1' WHERE bid = " + badgeID[0].bid;
			db.Execute(query);
		}
	}
}
