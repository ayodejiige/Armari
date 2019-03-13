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
    [Register ("CalendarCollectionViewController")]
    partial class CalendarCollectionViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        armari.CalendarCollectionView CalendarCollectionView_ { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CalendarCollectionView_ != null) {
                CalendarCollectionView_.Dispose ();
                CalendarCollectionView_ = null;
            }
        }
    }
}