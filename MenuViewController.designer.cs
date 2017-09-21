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
    [Register ("MenuViewController")]
    partial class MenuViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton badgeBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton mapBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel menuTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton qrBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton settingsBtn { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (badgeBtn != null) {
                badgeBtn.Dispose ();
                badgeBtn = null;
            }

            if (mapBtn != null) {
                mapBtn.Dispose ();
                mapBtn = null;
            }

            if (menuTitle != null) {
                menuTitle.Dispose ();
                menuTitle = null;
            }

            if (qrBtn != null) {
                qrBtn.Dispose ();
                qrBtn = null;
            }

            if (settingsBtn != null) {
                settingsBtn.Dispose ();
                settingsBtn = null;
            }
        }
    }
}