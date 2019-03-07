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
    [Register ("ClassCollectionViewController")]
    partial class ClassCollectionViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        armari.ClassCollectionView ClassCollectionView_ { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationItem ClassNavigation { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ClassCollectionView_ != null) {
                ClassCollectionView_.Dispose ();
                ClassCollectionView_ = null;
            }

            if (ClassNavigation != null) {
                ClassNavigation.Dispose ();
                ClassNavigation = null;
            }
        }
    }
}