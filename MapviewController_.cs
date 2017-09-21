using System;
using UIKit;
using MapKit;
using CoreLocation;
using SQLite;
using System.IO;
using System.Collections.Generic;
using ObjCRuntime;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;
using CoreGraphics;

namespace MagpieIOS
{
    public partial class MapviewController : UIViewController
    {

		private UIButton _annotationDetailButton;

		protected string annotationIdentifier = "AnnotationIdentifier";
		IMKAnnotation[] annoList = new IMKAnnotation[22];
		List<CLLocationCoordinate2D> annotationScrollList;
		int currentAnno;
		CLLocationCoordinate2D currAnnoLocation;
		bool locationUPDATED = false;
        public String selected = "-1";


        public void setSelected(String s) {
            this.selected = s;
        }


        public MapviewController (IntPtr handle) : base (handle)
        {
			annotationScrollList = new List<CLLocationCoordinate2D>();
			currentAnno = 0;
        }

        public MapviewController() {
            annotationScrollList = new List<CLLocationCoordinate2D>();
            currentAnno = 0;
        }

		public override void ViewDidLoad()
		{

            
			base.ViewDidLoad();


            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine(documents, "db_sqlite-net.db");
			var db = new SQLiteConnection(dbPath);
			var table = db.Table<MagpieBadge>();

			foreach (MagpieBadge badge in table)
			{
				CLLocationCoordinate2D c = new CLLocationCoordinate2D(badge.lat, badge.lon);
				annotationScrollList.Add(c);
			}

			annotationScrollList.Sort((x, y) => x.Longitude.CompareTo(y.Longitude));
			addAnnotations(table);
			initializeMap();

			mapToggle.ValueChanged += (s, e) =>
			{
				switch (mapToggle.SelectedSegment)
				{
					case 0:
						map.MapType = MKMapType.Standard;
						break;
					case 1:
						map.MapType = MKMapType.Satellite;
						break;
					case 2:
						map.MapType = MKMapType.Hybrid;
						break;
				}
			};


			CollectionDistance.Layer.BorderColor = UIColor.Gray.CGColor;
			CollectionDistance.Layer.BorderWidth = (System.nfloat).5;

            viewBadgeBtn.TouchUpInside += (s, e) => {
                goToBadge();
            };

		}


        private void initializeMap()
		{
			CLLocationManager locationManager = new CLLocationManager();
			locationManager.RequestWhenInUseAuthorization();
			locationManager.RequestAlwaysAuthorization(); //requests permission for access to location data while running in the background

			map.ShowsUserLocation = true;
            
			map.DidUpdateUserLocation += (sender, e) =>
			{
				if (map.UserLocation != null && !locationUPDATED && selected.Equals("-1"))
				{
					//this would constantly move map back to user location
					CLLocationCoordinate2D coords = map.UserLocation.Coordinate;
					MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(coords, 100, 100);
					map.Region = mapRegion;
					locationUPDATED = true;
				}
                if (!selected.Equals("-1"))
                {
                    getDistanceTime(currAnnoLocation, map.UserLocation.Coordinate);
                }

            };

			//if user location not accesible start at this location
			if (!map.UserLocationVisible)
			{
				CLLocationCoordinate2D mapCenter = new CLLocationCoordinate2D(47.658779, -117.426048);
				MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(mapCenter, 100, 100);
				map.CenterCoordinate = mapCenter;
				map.Region = mapRegion;
			}

			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine(documents, "db_sqlite-net.db");
			var db = new SQLiteConnection(dbPath);

            if (selected.Equals("-1"))
            {

                viewBadgeBtn.Enabled = false;
                SculptureTitle.Text = "Your location";
                SculptureSubtitle.Text = "";
                DistanceAway.Text = "0";
                milesTo.Text = "0";
                milesToLabel.Hidden = true;
                DistanceAwayLabel.Hidden = true;
                milesTo.Hidden = true;
                DistanceAway.Hidden = true;
            }
            else {

                var startInfo = db.Query<MagpieBadge>("SELECT * FROM MagpieBadge WHERE bid = "+ selected);
                SculptureTitle.Text = startInfo[0].bname;
                SculptureSubtitle.Text = startInfo[0].art + " - " + startInfo[0].year;
                CLLocationCoordinate2D mapCenter = new CLLocationCoordinate2D(startInfo[0].lat, startInfo[0].lon);
                map.SetCenterCoordinate(mapCenter, true);
                currAnnoLocation = mapCenter;
                viewBadgeBtn.Enabled = true;
                int i = 0;
                foreach (CLLocationCoordinate2D a in annotationScrollList) {

                    if (a.Latitude.Equals(currAnnoLocation.Latitude))
                    {
                        currentAnno = i;
                        break;
                    }
                    else {
                        i++;
                    }
                }
            }



            //styling
            SculptureTitle.Font = UIFont.FromName("Montserrat-Light", 19f);   
            SculptureSubtitle.Font = UIFont.FromName("Montserrat-Medium", 18f);
            SculptureSubtitle.TextColor = UIColor.Gray;
            milesTo.Font = UIFont.FromName("Montserrat-Medium", 20f);
            DistanceAway.Font = UIFont.FromName("Montserrat-Medium", 20f);
            milesToLabel.Font = UIFont.FromName("Montserrat-Light", 20f);
            DistanceAwayLabel.Font = UIFont.FromName("Montserrat-Light", 20f);
            milesToLabel.TextColor = UIColor.Gray;
            DistanceAwayLabel.TextColor = UIColor.Gray;
            //end styling


            map.GetViewForAnnotation = GetViewForAnnotation;

			map.DidSelectAnnotationView += (object sender, MKAnnotationViewEventArgs e) =>
			{
				annoList = map.SelectedAnnotations;
				foreach (IMKAnnotation a in annoList)
				{
					SculptureTitle.Text = a.GetTitle();
                    SculptureSubtitle.Text = a.GetSubtitle();
                    getDistanceTime(a.Coordinate, map.UserLocation.Coordinate);
					currAnnoLocation = new CLLocationCoordinate2D(a.Coordinate.Latitude, a.Coordinate.Longitude);
                    viewBadgeBtn.Enabled = true;
                    int i = 0;
                    foreach (CLLocationCoordinate2D annot in annotationScrollList)
                    {

                        if (annot.Longitude.Equals(currAnnoLocation.Longitude))
                        {
                            currentAnno = i;
                            if (annot.Longitude.Equals(a.Coordinate.Longitude)) {
                                AnnotationModel m = a as AnnotationModel;
                                this.selected = m.GetID().ToString();
                            }
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
			};

			LeftAnnotation.TouchUpInside += (sender, e) =>
			{
				CLLocationCoordinate2D anno = annotationScrollList[currentAnno];

				map.SetCenterCoordinate(anno, true);

				getDistanceTime(anno, map.UserLocation.Coordinate);

				var table = db.Query<MagpieBadge>("SELECT * FROM MagpieBadge WHERE lon = " + anno.Longitude);
				SculptureTitle.Text = table[0].bname;
                SculptureSubtitle.Text = table[0].art + " - " + table[0].year;
                this.selected = table[0].bid;

                if (currentAnno == 0)
				{
					currentAnno = annotationScrollList.Count - 1;
				}
				else
				{
					currentAnno--;
				}

                viewBadgeBtn.Enabled = true;
			};

			RightAnnotation.TouchUpInside += (sender, e) =>
			{
				CLLocationCoordinate2D anno = annotationScrollList[currentAnno];
				
				map.SetCenterCoordinate(anno, true);

				getDistanceTime(anno, map.UserLocation.Coordinate);

				var table = db.Query<MagpieBadge>("SELECT * FROM MagpieBadge WHERE lon = " + anno.Longitude);
				SculptureTitle.Text = table[0].bname;
                SculptureSubtitle.Text = table[0].art + " - " + table[0].year;
                this.selected = table[0].bid;

                if (currentAnno == annotationScrollList.Count - 1)
				{
					currentAnno = 0;
				}
				else
				{
					currentAnno++;
				}
                viewBadgeBtn.Enabled = true;
			};


			_btnCurrentLocation.TouchUpInside += (sender, e) =>
			{
				map.SetCenterCoordinate(map.UserLocation.Location.Coordinate, true);
                currentAnno = 0;
            
                SculptureTitle.Text = "Your location";
                SculptureSubtitle.Text = "";
                DistanceAway.Text = "0";
                milesTo.Text = "0";

                DistanceAway.Hidden = true; 
                milesTo.Hidden = true;
                milesToLabel.Hidden = true;
                DistanceAwayLabel.Hidden = true;

                viewBadgeBtn.Enabled = false;

                selected = "-1";
            };
			View.AddSubview(_btnCurrentLocation);
		}


        private void addAnnotations(SQLite.TableQuery<MagpieIOS.MagpieBadge> table)
		{
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine(documents, "db_sqlite-net.db");
			var db = new SQLiteConnection(dbPath);


			int index = 0;

			foreach (MagpieBadge badge in table)
			{
				double lat = table.ElementAt(index).lat;
				double lon = table.ElementAt(index).lon;
				String name = table.ElementAt(index).bname;
                String sub = table.ElementAt(index).art + " - " + table.ElementAt(index).year;

                //if iscollected check
                String q = "SELECT * FROM MagpieUser WHERE bid = " + table.ElementAt(index).bid;
				var userTuple = db.Query<MagpieUser>(q);

				if (userTuple.ElementAt(0).isClaimed.Equals(""))
				{
					var annotation = new AnnotationModel(new CLLocationCoordinate2D(lat, lon), name, index, false, sub);
					map.AddAnnotation(annotation);
				}
				else
				{
					var annotation = new AnnotationModel(new CLLocationCoordinate2D(lat, lon), name, index, true, sub);
					map.AddAnnotation(annotation);
				}

				index++;

			}

		}


		MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = mapView.DequeueReusableAnnotation(annotationIdentifier);
			// Set current location and location of annotation
			CLLocationCoordinate2D currentLocation = mapView.UserLocation.Coordinate;
			CLLocationCoordinate2D annotationLocation = annotation.Coordinate;

			// We don't want a special annotation for the user location
			if (currentLocation.Latitude == annotationLocation.Latitude && currentLocation.Longitude == annotationLocation.Longitude)
				return null;

			if (annotationView == null)
			{
				annotationView = new MKPinAnnotationView(annotation, annotationIdentifier);
			}
			else
				annotationView.Annotation = annotation;


			annotationView.CanShowCallout = true;
			(annotationView as MKPinAnnotationView).AnimatesDrop = false; // Set to true if you want to animate the pin dropping

			if ((annotation as AnnotationModel).isClaimed())
			{
				(annotationView as MKPinAnnotationView).PinColor = MKPinAnnotationColor.Green;
			}
			else
			{
				(annotationView as MKPinAnnotationView).PinColor = MKPinAnnotationColor.Red;
			}

			annotationView.SetSelected(true, false);

			annotationView.Annotation.GetTitle();
			_annotationDetailButton = UIButton.FromType(UIButtonType.DetailDisclosure);
			_annotationDetailButton.TouchUpInside += (sender, e) =>
			{

				this.PerformSegue("ShowView", this);
			};

			annotationView.Image = UIImage.FromBundle("images/Icon-Small.png");

			annotationView.RightCalloutAccessoryView = _annotationDetailButton;

			// Annotation icon may be specified like this, in case you want it.
			annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromBundle("images/Icon-Small.png"));
			return annotationView;
		}

		public void getDistanceTime(CLLocationCoordinate2D destination, CLLocationCoordinate2D current)
		{
			CLLocation dest = new CLLocation(destination.Latitude, destination.Longitude);
			CLLocation cur = new CLLocation(current.Latitude, current.Longitude);

			//get and display distance to location
			double meters = cur.DistanceFrom(dest);
			double feet = meters * 3.28084;
			double miles = feet / 5280;
			String distance = String.Format("{0:0.00}", miles);
			milesTo.Text = distance;
            milesToLabel.Hidden = false;
            milesTo.Hidden = false;

			//estimate and display time to location, assuming 1.4 m/s average pace
			double seconds = meters * 1.4;
			double minutes = seconds / 60;
			string time = String.Format("{0:0.00}", minutes);
			DistanceAway.Text = time;
            DistanceAwayLabel.Hidden = false;
            DistanceAway.Hidden = false;
		}        

        public void goToBadge()
        {

            //Creates badge detail image -- need assets
            UIImageView image = new UIImageView(UIImage.FromFile("defaultBadge.png"));

            UIViewController badgeDetailView = new UIViewController();
            badgeDetailView.EdgesForExtendedLayout = UIRectEdge.None;
            badgeDetailView.Add(image);

            //Create badge title bar
            UILabel label = new UILabel(new System.Drawing.RectangleF(0, 230, (float)UIScreen.MainScreen.Bounds.Width, 60));
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(documents, "db_sqlite-net.db");
            var db = new SQLiteConnection(dbPath);
            var table = db.Table<MagpieBadge>();
            CLLocationCoordinate2D anno = annotationScrollList[currentAnno];
            var query = db.Query<MagpieBadge>("SELECT * FROM MagpieBadge WHERE lon = " + anno.Longitude);

            label.Text = query[0].bname;
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
                this.selected = query[0].bid;
                NavigationController.PopViewController(true);
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
            string parsed = Regex.Replace(query[0].bname, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);


            String q = "SELECT * FROM MagpieUser WHERE bid = " + query[0].bid;
            System.Collections.Generic.List<MagpieUser> userTuple = db.Query<MagpieUser>(q);

            
            UIButton badgeBtn = new UIButton(new CoreGraphics.CGRect((float)(UIScreen.MainScreen.Bounds.Width / 2) - 75, 270, 150, 128.89221556886));
            UILabel badgeLabel = new UILabel(new CGRect(0, 50, badgeBtn.Bounds.Width, badgeBtn.Bounds.Height / 4));
            badgeLabel.UserInteractionEnabled = false;

            if (userTuple.ElementAt(0).isClaimed.Equals("")) //user does not have badge
            {
                if (checkDistance(new CLLocation(query[0].lat, query[0].lon), new CLLocation(map.UserLocation.Location.Coordinate.Latitude, map.UserLocation.Location.Coordinate.Longitude)))
                {
                    //user is within collection distance

                    parsed = "badges/SSW_" + parsed + ".png";
                    badgeLabel.Text = "COLLECT";

                }
                else
                {

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

                if (checkDistance(new CLLocation(query[0].lat, query[0].lon), new CLLocation(map.UserLocation.Location.Coordinate.Latitude, map.UserLocation.Location.Coordinate.Longitude)))
                {
                    //user can collect
                    badgeLabel.Text = "";
                    badgeLabel.BackgroundColor = UIColor.Clear;
                    ClaimBadge claim = new ClaimBadge();
                    claim.claimLocalBadge(new CLLocationCoordinate2D(query[0].lat, query[0].lon));
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
            subLabel.Text = "ARTIST: " + query[0].art.ToUpper();
            subLabel.TextColor = UIColor.LightGray;
            subLabel.Font = UIFont.FromName("Montserrat-Bold", 16f);

            //Builds the badge description
            UITextView description = new UITextView(new System.Drawing.RectangleF(20, 20, (float)(textScroll.Bounds.Width - 40), (float)textScroll.Bounds.Height));
            description.Add(subLabel);
            description.Text = "\n\n" + query[0].desc;

            description.TextColor = UIColor.Black;
            description.Font = UIFont.FromName("Montserrat-Regular", 16f);

            textScroll.AddSubview(description);

            badgeDetailView.Add(textScroll);
            //End badge description build


            //show badge detail view controller
            this.NavigationController.PushViewController(badgeDetailView, true);

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