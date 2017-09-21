// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MagpieIOS
{
    [Register ("ScannerView")]
    partial class ScannerView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton scan { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (scan != null) {
                scan.Dispose ();
                scan = null;
            }
        }
    }
}