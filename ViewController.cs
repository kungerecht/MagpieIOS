using System;

using UIKit;

namespace MagpieIOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            /*
            //Get size of screen
            var height = UIScreen.MainScreen.Bounds.Height;
            var width = UIScreen.MainScreen.Bounds.Width;

            //Labels
            var lab1 = new UILabel(new RectangleF(0, 0, width, height / 2));
            lab1.Text = "This is some sample text";
            lab1.Font = UIFont.FromName("HollywoodHills", 20f);
            this.Add(lab1);
            */

            //this.titleLogin.Font = UIFont.FromName("Montserrat-Medium", 35f);

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}