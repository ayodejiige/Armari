// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace armari
{
    [Register ("DayCollectionViewController")]
    partial class DayCollectionViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        armari.DayCollectionView DayCollectionView_ { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView DayScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GenerateButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DayCollectionView_ != null) {
                DayCollectionView_.Dispose ();
                DayCollectionView_ = null;
            }

            if (DayScrollView != null) {
                DayScrollView.Dispose ();
                DayScrollView = null;
            }

            if (GenerateButton != null) {
                GenerateButton.Dispose ();
                GenerateButton = null;
            }
        }
    }
}