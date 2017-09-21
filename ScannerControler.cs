using Foundation;
using System;
using UIKit;

namespace MagpieIOS
{
    public partial class ScannerControler : UIViewController
    {
        public ScannerControler (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			scan();
		}

		public async void scan() {
			
			var scanner = new ZXing.Mobile.MobileBarcodeScanner();
			Console.WriteLine("Attempting Scan");
			var result = await scanner.Scan();

			if (result != null)
			{
				Console.WriteLine("Scanned Barcode: " + result.Text);
			}
		}
    }
}