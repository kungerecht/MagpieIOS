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
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel accountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField accountName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton legalBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel locationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField locationName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton logoutBtn { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (accountLabel != null) {
                accountLabel.Dispose ();
                accountLabel = null;
            }

            if (accountName != null) {
                accountName.Dispose ();
                accountName = null;
            }

            if (legalBtn != null) {
                legalBtn.Dispose ();
                legalBtn = null;
            }

            if (locationLabel != null) {
                locationLabel.Dispose ();
                locationLabel = null;
            }

            if (locationName != null) {
                locationName.Dispose ();
                locationName = null;
            }

            if (logoutBtn != null) {
                logoutBtn.Dispose ();
                logoutBtn = null;
            }
        }
    }
}