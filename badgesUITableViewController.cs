using Foundation;
using System;
using UIKit;
using CoreLocation;
using System.IO;
using SQLite;
using System.Linq;

namespace MagpieIOS
{
    public partial class badgesUITableViewController : UITableViewController
    {
        public MenuViewController menu;
        public string selectedBid = "-1";

        public badgesUITableViewController (IntPtr handle) : base (handle)
        {
            var nav = ParentViewController;
            menu = (MenuViewController)ParentViewController.ChildViewControllers[0];
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            /* code for the database information
			 * 
			 * connect to local sqlite database
			 *  var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var dbPath = Path.Combine(documents, "db_sqlite-net.db");
				var db = new SQLiteConnection(dbPath);

				//example of using specific query
				var startInfo = db.Query<MagpieBadge>("SELECT * FROM MagpieBadge WHERE bid = 1");
			    SculptureTitle.Text = startInfo[0].bname;

				//example of getting whole table, use for each to get to items
				var table = db.Table<MagpieBadge>();
				foreach (MagpieBadge badge in table)
			{
				double lat = table.ElementAt(index).lat;
				double lon = table.ElementAt(index).lon;
				String name = table.ElementAt(index).bname;

			 * 
			 */


            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(documents, "db_sqlite-net.db");
            var db = new SQLiteConnection(dbPath);


            Tuple<string, string, string, string, CLLocationCoordinate2D>[] statues = new Tuple<string, string, string, string, CLLocationCoordinate2D>[22];
            int i = 0;
            Tuple<string, string, string, string, CLLocationCoordinate2D> s;

            var table = db.Table<MagpieBadge>();
            foreach (MagpieBadge badge in table)
            {
                if (i < 21)
                {
                    String name = badge.bname;
                    String artist = badge.art;
                    String year = badge.year;
                    artist += " - " + year;
                    String description = badge.desc;
                    String id = badge.bid;
                    CLLocationCoordinate2D loc = new CLLocationCoordinate2D(badge.lat, badge.lon);

                    s = new Tuple<string, string, string, string, CLLocationCoordinate2D>(name, artist, description, id, loc);
                    statues[i] = s;
                    i++;
                }
                else
                {
                    break;
                }
            }
            s = new Tuple<string, string, string, string, CLLocationCoordinate2D>("", "", "", "", new CLLocationCoordinate2D());
            statues[i] = s;

            this.badgeTableView = new UITableView(new System.Drawing.RectangleF(0, 0, (float)UIScreen.MainScreen.Bounds.Width, (float)(UIScreen.MainScreen.Bounds.Height)));
            this.badgeTableView.RowHeight = 100f;
            badgeTableView.Source = new TableSource(statues, this);
            Add(badgeTableView);

        }
        

    }
}