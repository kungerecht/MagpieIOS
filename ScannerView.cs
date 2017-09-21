using Foundation;
using System;
using UIKit;
using ZXing;

namespace MagpieIOS
{
    public partial class ScannerView : UIViewController
    {
        public ScannerView (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			scan.TouchUpInside += async (sender, e) =>
			{
				var scanner = new ZXing.Mobile.MobileBarcodeScanner();
				Console.WriteLine("Attempting Scan");
				var result = await scanner.Scan();

				if (result != null)
				{
					Console.WriteLine("Scanned Barcode: " + result.Text);
				}
			};
		}
    }
}