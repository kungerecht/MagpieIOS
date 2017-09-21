using System;
using System.IO;
using System.Net;
using CoreLocation;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SQLite;

namespace MagpieIOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
		private CLLocationManager _locationManager;
		Stream objStream;

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            _locationManager = new CLLocationManager();

			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
				_locationManager.RequestWhenInUseAuthorization();

			// Access data and store in local database
			string request = "http://www.zjohns.com/magpie/magpie.php?";
			WebRequest wrequest = WebRequest.Create(request);
			Stream objStream = wrequest.GetResponse().GetResponseStream();
			StreamReader objReader = new StreamReader(objStream);

			string sLine = objReader.ReadLine();
			var arr = JsonConvert.DeserializeObject<List<MagpieUser>>(sLine);
			List<MagpieBadge> arr2 = new List<MagpieBadge>();

			for (int i = 0; i < arr.Count; i++)
			{
				int bid_temp = arr.ElementAt(i).bid;
				request = "http://www.zjohns.com/magpie/magpie.php?Badge_ID=" + bid_temp;
				wrequest = WebRequest.Create(request);
				objStream = wrequest.GetResponse().GetResponseStream();
				objReader = new StreamReader(objStream);
				sLine = objReader.ReadLine();
				MagpieBadge badge_temp = JsonConvert.DeserializeObject<MagpieBadge>(sLine);
				arr2.Add(badge_temp);
			}

			//attempt to make db
			DeleteDatabaseIfItAlreadyExists();
			Console.WriteLine("Attempting to create db");
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine(documents, "db_sqlite-net.db");
			var db = new SQLiteConnection(dbPath);
			db.CreateTable<MagpieBadge>();
			foreach (MagpieBadge badge in arr2)
			{
				db.Insert(badge);
			}

			db.CreateTable<MagpieUser>();
			foreach (MagpieUser user in arr)
			{
				db.Insert(user);
			}


			return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.

			Console.WriteLine("App entering background state.");

			//get curretn local db table informatin for users
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine(documents, "db_sqlite-net.db");
			var db = new SQLiteConnection(dbPath);
			var table = db.Table<MagpieUser>();


			//loop through table and update all 
			foreach (MagpieUser user in table)
			{
				Console.WriteLine("BID: " + user.bid + ", Claimed: " + user.isClaimed);

				if (user.isClaimed.Equals("1"))
				{
					//make call to hosted db and update all bidsClaimed badges
					WebRequest wrequest = WebRequest.Create("http://www.zjohns.com/magpie/update.php?Badge_ID=" + user.bid);
					objStream = wrequest.GetResponse().GetResponseStream();
				}
			}
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
			Console.WriteLine("App will enter foreground");
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
					//get curretn local db table informatin for users
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine(documents, "db_sqlite-net.db");
			var db = new SQLiteConnection(dbPath);
			var table = db.Table<MagpieUser>();



			//loop through table and get all updated bid's 
			foreach (MagpieUser user in table)
			{
				Console.WriteLine("BID: " + user.bid + ", Claimed: " + user.isClaimed);

				if (user.isClaimed.Equals("1"))
				{
					//make call to hosted db and update all bidsClaimed badges
					WebRequest wrequest = WebRequest.Create("http://www.zjohns.com/magpie/update.php?Badge_ID=" + user.bid);
					objStream = wrequest.GetResponse().GetResponseStream();
				}
			}
        }

		private void DeleteDatabaseIfItAlreadyExists()
		{
			var dbName = "db_sqlite-net.db";
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine(documents, dbName);

			if (File.Exists(dbPath))
			{
				File.Delete(dbPath);
			}
		}
    }
}