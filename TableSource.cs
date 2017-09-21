using CoreGraphics;
using CoreLocation;
using Foundation;
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UIKit;

namespace MagpieIOS
{
    internal class TableSource : UITableViewSource
    {
        public Tuple<string,string, string, string, CLLocationCoordinate2D>[] statues; //<title, artist, description, id, location>
        string CellIdentifier = "badgeCell";
        badgesUITableViewController owner;
        System.Collections.Generic.List<MagpieUser> userTuple;
        bool selectTheRow;
		CLLocationManager user;
		CLLocation currentLoc;
        UILabel badgeLabel = new UILabel();
        
        int indexRow = -1;

        public TableSource(Tuple<string, string, string, string, CLLocationCoordinate2D>[] statues, badgesUITableViewController owner)
        {
            this.owner = owner;
            this.statues = statues;
            badgeLabel.Text = "";

            user = new CLLocationManager();
            
			user.LocationsUpdated += (sender, e) =>
			{
				foreach (var loc in e.Locations)
				{
					Console.WriteLine(loc);
					currentLoc = loc;
                    if (indexRow > -1)
                    {
                        if (checkDistance(new CLLocation(statues[indexRow].Item5.Latitude, statues[indexRow].Item5.Longitude), currentLoc))
                        {
                            //close enough
                            if (!badgeLabel.Text.Equals(""))
                            {
                                badgeLabel.Text = "COLLECT";
                            }

                        }
                    }
                }
			};
			user.StartUpdatingLocation();
        }

        public TableSource(Tuple<string, string, string, string, CLLocationCoordinate2D>[] statues, badgesUITableViewController owner, bool b)
        {
            this.owner = owner;
            this.statues = statues;
            selectTheRow = b;
            badgeLabel.Text = "";

            user = new CLLocationManager();

            user.LocationsUpdated += (sender, e) =>
            {
                foreach (var loc in e.Locations)
                {
                    Console.WriteLine(loc);
                    currentLoc = loc;
                    if (indexRow > -1)
                    {
                        if (checkDistance(new CLLocation(statues[indexRow].Item5.Latitude, statues[indexRow].Item5.Longitude), currentLoc))
                        {
                            //close enough
                            if (!badgeLabel.Text.Equals(""))
                            {
                                badgeLabel.Text = "COLLECT";
                            }
                            

                        }
                    }
                }
            };
            user.StartUpdatingLocation();

        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return statues.Length;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
        //creates each table cell dynamically

            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            string item = statues[indexPath.Row].Item1;
            string sub = statues[indexPath.Row].Item2;

        //if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier); }

            if (item.Equals(""))
            {
                cell.UserInteractionEnabled = false;
                cell.TextLabel.Text = item;
                cell.DetailTextLabel.Text = sub;
                cell.ImageView.SizeToFit();
                cell.TextLabel.Font = UIFont.FromName("Montserrat-Light", 17f);
                cell.DetailTextLabel.Font = UIFont.FromName("Montserrat-Bold", 12f);
                cell.DetailTextLabel.TextColor = UIColor.LightGray;
                cell.ImageView.Hidden = true;
                return cell;
            }

            //parse the statue's title into a file name
            string parsed = Regex.Replace(item, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(documents, "db_sqlite-net.db");
            var db = new SQLiteConnection(dbPath);

            String q = "SELECT * FROM MagpieUser WHERE bid = " + statues[indexPath.Row].Item4;
            userTuple = db.Query<MagpieUser>(q);

            if (userTuple.ElementAt(0).isClaimed.Equals("")) //user does not have badge
            {
                parsed = "graybadges/SSW_" + parsed + ".png";
            }
            else //user has badge
            {
                parsed = "badges/SSW_" + parsed + ".png";
            }

            //create badge icon
            var img = UIImage.FromFile(parsed);
            cell.ImageView.Image = img;
            cell.ImageView.SizeToFit();

            cell.TextLabel.Text = item;
            cell.DetailTextLabel.Text = sub;

        //title text styling *
            cell.TextLabel.Font = UIFont.FromName("Montserrat-Light", 17f);
            cell.DetailTextLabel.Font = UIFont.FromName("Montserrat-Bold", 12f);
            cell.DetailTextLabel.TextColor = UIColor.LightGray;
            cell.ImageView.Hidden = false;
            cell.UserInteractionEnabled = true;

            if (selectTheRow) { cell.Select(owner); }
            return cell;

            
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //Event when a badge table row is selected

            indexRow = indexPath.Row;


      //Creates badge detail image -- need assets
            UIImageView image = new UIImageView(UIImage.FromFile("defaultBadge.png"));

            UIViewController badgeDetailView = new UIViewController();
            badgeDetailView.EdgesForExtendedLayout = UIRectEdge.None;
            badgeDetailView.Add(image);

      //Create badge title bar
            UILabel label = new UILabel(new System.Drawing.RectangleF(0, 230, (float)UIScreen.MainScreen.Bounds.Width, 60));
            label.Text = statues[indexPath.Row].Item1;
            if (label.Text.Length < 30)
            {
                label.Font = UIFont.FromName("Montserrat-Light", 24f);
            }
            else
            {
                label.Font = UIFont.FromName("Montserrat-Light", 18f);
            }
            label.TextColor = UIColor.White;
            label.BackgroundColor = UIColor.LightGray;
            label.TextAlignment = UITextAlignment.Center;
            badgeDetailView.Add(label);
      //end badge title bar

      //Holds the view map button and share button
            UIView buttonBar = new UIView(new System.Drawing.RectangleF(0, 290, (float)UIScreen.MainScreen.Bounds.Width, 90));
            buttonBar.BackgroundColor = UIColor.DarkGray;

      //Build map button
            UIButton viewInMapBtn = new UIButton(new System.Drawing.RectangleF(50, 15, 40, 40));
            viewInMapBtn.SetImage(UIImage.FromFile("mapIcon.png"), UIControlState.Normal);

            UILabel mapBtnLabel = new UILabel(new System.Drawing.RectangleF(20, 60, 100, 20));
            mapBtnLabel.Text = "VIEW IN MAP";
            mapBtnLabel.TextAlignment = UITextAlignment.Center;
            mapBtnLabel.Font = UIFont.FromName("Montserrat-Bold", 12f);
            mapBtnLabel.TextColor = UIColor.White;
            buttonBar.Add(mapBtnLabel);

            viewInMapBtn.TouchUpInside += (object sender, EventArgs e) =>
            {
                owner.menu.selectedBid = statues[indexPath.Row].Item4;
                owner.menu.PerformSegue("ShowMap", (NSObject)sender);
            };

            buttonBar.Add(viewInMapBtn);
      //end map button

      //Build share button
            UIButton shareBtn = new UIButton(new System.Drawing.RectangleF((float)(UIScreen.MainScreen.Bounds.Width - 90), 15, 40, 40));
            shareBtn.SetImage(UIImage.FromFile("shareIcon.png"), UIControlState.Normal);

            UILabel shareBtnLabel = new UILabel(new System.Drawing.RectangleF((float)(UIScreen.MainScreen.Bounds.Width - 100), 60, 60, 20));
            shareBtnLabel.Text = "SHARE";
            shareBtnLabel.TextAlignment = UITextAlignment.Center;
            shareBtnLabel.Font = UIFont.FromName("Montserrat-Bold", 12f);
            shareBtnLabel.TextColor = UIColor.White;
            buttonBar.Add(shareBtnLabel);

            buttonBar.Add(shareBtn);
     //end share button

     //Create badge icon
            string parsed = Regex.Replace(statues[indexPath.Row].Item1, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(documents, "db_sqlite-net.db");
            var db = new SQLiteConnection(dbPath);

            String q = "SELECT * FROM MagpieUser WHERE bid = " + statues[indexPath.Row].Item4;
            userTuple = db.Query<MagpieUser>(q);

            UIButton badgeBtn = new UIButton(new CoreGraphics.CGRect((float)(UIScreen.MainScreen.Bounds.Width / 2) - 75, 270, 150, 128.89221556886));
            badgeLabel = new UILabel(new CGRect(0, 50, badgeBtn.Bounds.Width, badgeBtn.Bounds.Height / 4));
            badgeLabel.UserInteractionEnabled = false;

            if (userTuple.ElementAt(0).isClaimed.Equals("")) //user does not have badge
            {
                if (checkDistance(new CLLocation(statues[indexPath.Row].Item5.Latitude, statues[indexPath.Row].Item5.Longitude), currentLoc))
                {
                    //user is within collection distance

                    parsed = "badges/SSW_" + parsed + ".png";
                    badgeLabel.Text = "COLLECT";

                }
                else {

                    parsed = "graybadges/SSW_" + parsed + ".png";
                    badgeLabel.Text = "MOVE CLOSER";
                }
                badgeLabel.Font = UIFont.FromName("Montserrat-Bold", 14f);
                badgeLabel.TextColor = UIColor.White;
                badgeLabel.TextAlignment = UITextAlignment.Center;
                badgeLabel.BackgroundColor = UIColor.DarkGray.ColorWithAlpha(0.5f);
            }
            else //user has badge
            {
                parsed = "badges/SSW_" + parsed + ".png";
                badgeBtn.UserInteractionEnabled = false;
            }

            UIImageView icon = new UIImageView(UIImage.FromFile(parsed));
            icon.Frame = new CoreGraphics.CGRect((float)(UIScreen.MainScreen.Bounds.Width / 2) - 75, 270, 150, 128.89221556886);
            icon.UserInteractionEnabled = false;

            icon.AddSubview(badgeLabel);

            badgeDetailView.Add(buttonBar);

            badgeBtn.TouchUpInside += (object sender, EventArgs e) =>
            {
                //Collect badge event
				if (checkDistance(new CLLocation(statues[indexPath.Row].Item5.Latitude, statues[indexPath.Row].Item5.Longitude), currentLoc))
                {
                    //user can collect
                    badgeLabel.Text = "";
                    badgeLabel.BackgroundColor = UIColor.Clear;
                    ClaimBadge claim = new ClaimBadge();
                    claim.claimLocalBadge(statues[indexPath.Row].Item5);
                }
            };

            badgeDetailView.Add(icon);
            badgeDetailView.Add(badgeBtn);

            //Controls the scroll view of the description and artist
            UIScrollView textScroll = new UIScrollView(new System.Drawing.RectangleF(0, 380, (float)UIScreen.MainScreen.Bounds.Width, (float)UIScreen.MainScreen.Bounds.Height - 440));
            textScroll.ScrollEnabled = true;
            textScroll.BackgroundColor = UIColor.White;

      //Build artist and year description
            UILabel subLabel = new UILabel(new System.Drawing.RectangleF(5, 0, (float)textScroll.Bounds.Width, 40));
            subLabel.Text = "ARTIST: " + statues[indexPath.Row].Item2.ToUpper();
            subLabel.TextColor = UIColor.LightGray;
            subLabel.Font = UIFont.FromName("Montserrat-Bold", 16f);

      //Builds the badge description
            UITextView description = new UITextView(new System.Drawing.RectangleF(20, 20, (float)(textScroll.Bounds.Width - 40), (float)textScroll.Bounds.Height));
            description.Add(subLabel);
            description.Text = "\n\n" + statues[indexPath.Row].Item3;
            description.UserInteractionEnabled = false;

            description.TextColor = UIColor.Black;
            description.Font = UIFont.FromName("Montserrat-Regular", 16f);

            textScroll.AddSubview(description);

            badgeDetailView.Add(textScroll);
     //End badge description build



            //show badge detail view controller
            owner.NavigationController.PushViewController(badgeDetailView, true);

            /*
             * creates popup for debugging
             * 
            UIAlertController okAlertController = UIAlertController.Create("Row Selected", statues[indexPath.Row].Item1 + "\n" + statues[indexPath.Row].Item2, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            owner.PresentViewController(okAlertController, true, null);
            */

            tableView.DeselectRow(indexPath, true);

            //loop for badge button updating
			/*
            while (!checkDistance(new CLLocation(statues[indexPath.Row].Item5.Latitude, statues[indexPath.Row].Item5.Longitude), currentLoc)) {
                //not close enough
                badgeLabel.Text = "MOVE CLOSER";
            }
            if (checkDistance(new CLLocation(statues[indexPath.Row].Item5.Latitude, statues[indexPath.Row].Item5.Longitude), currentLoc)) {
                //close enough
                badgeLabel.Text = "COLLECT";

            }
			*/
        }

		public bool checkDistance(CLLocation sculpture, CLLocation current)
		{
			double meters = current.DistanceFrom(sculpture);
			if (meters <= 7)
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
