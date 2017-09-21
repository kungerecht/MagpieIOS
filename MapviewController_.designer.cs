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
    [Register ("MapviewController")]
    partial class MapviewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton _btnCurrentLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CollectionDistance { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DistanceAway { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DistanceAwayLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LeftAnnotation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView map { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl mapToggle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel milesTo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel milesToLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RightAnnotation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SculptureSubtitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SculptureTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton viewBadgeBtn { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_btnCurrentLocation != null) {
                _btnCurrentLocation.Dispose ();
                _btnCurrentLocation = null;
            }

            if (CollectionDistance != null) {
                CollectionDistance.Dispose ();
                CollectionDistance = null;
            }

            if (DistanceAway != null) {
                DistanceAway.Dispose ();
                DistanceAway = null;
            }

            if (DistanceAwayLabel != null) {
                DistanceAwayLabel.Dispose ();
                DistanceAwayLabel = null;
            }

            if (LeftAnnotation != null) {
                LeftAnnotation.Dispose ();
                LeftAnnotation = null;
            }

            if (map != null) {
                map.Dispose ();
                map = null;
            }

            if (mapToggle != null) {
                mapToggle.Dispose ();
                mapToggle = null;
            }

            if (milesTo != null) {
                milesTo.Dispose ();
                milesTo = null;
            }

            if (milesToLabel != null) {
                milesToLabel.Dispose ();
                milesToLabel = null;
            }

            if (RightAnnotation != null) {
                RightAnnotation.Dispose ();
                RightAnnotation = null;
            }

            if (SculptureSubtitle != null) {
                SculptureSubtitle.Dispose ();
                SculptureSubtitle = null;
            }

            if (SculptureTitle != null) {
                SculptureTitle.Dispose ();
                SculptureTitle = null;
            }

            if (viewBadgeBtn != null) {
                viewBadgeBtn.Dispose ();
                viewBadgeBtn = null;
            }
        }
    }
}