using Foundation;
using System;
using UIKit;
using ZXing;

namespace MagpieIOS
{
    public partial class MenuViewController : UIViewController
    {

        public string selectedBid = "-1";
        public String username = "";

        public MenuViewController (IntPtr handle) : base (handle)
        {
        }

        public void setUsername(String s)
        {
            this.username = s;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

      //Style Menu buttons
            this.menuTitle.Font = UIFont.FromName("Montserrat-Medium", 35f);
            this.badgeBtn.Font = UIFont.FromName("Montserrat-Medium", 26f);
            this.mapBtn.Font = UIFont.FromName("Montserrat-Medium", 26f);
            this.qrBtn.Font = UIFont.FromName("Montserrat-Medium", 26f);
            this.settingsBtn.Font = UIFont.FromName("Montserrat-Medium", 26f);


        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            if (segue.Identifier != null)
            {
                if (segue.Identifier.Equals("ShowMap"))
                {
                    var viewController = (MapviewController)segue.DestinationViewController;
                    viewController.setSelected(selectedBid);
                }
            }
        }



    }
}