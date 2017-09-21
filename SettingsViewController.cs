using Foundation;
using System;
using UIKit;

namespace MagpieIOS
{
    public partial class SettingsViewController : UIViewController
    {

        public MenuViewController menu;

        public SettingsViewController (IntPtr handle) : base (handle)
        {
            var nav = ParentViewController;
            menu = (MenuViewController)ParentViewController.ChildViewControllers[0];
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            accountName.Text = menu.username;

            accountLabel.Font = UIFont.FromName("Montserrat-Light", 22f);
            accountName.Font = UIFont.FromName("Montserrat-Medium", 22f);
            accountName.TextColor = UIColor.Gray;
            locationLabel.Font = UIFont.FromName("Montserrat-Light", 22f);
            locationName.Font = UIFont.FromName("Montserrat-Medium", 22f);
            locationName.TextColor = UIColor.Gray;

            logoutBtn.Font = UIFont.FromName("Montserrat-Medium", 26f);
            legalBtn.Font = UIFont.FromName("Montserrat-Medium", 26f);

            logoutBtn.TouchUpInside += (sender, e) =>
            {
                this.NavigationController.DismissModalViewController(true);

            };
        }
    }
}