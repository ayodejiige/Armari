// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace armari
{
    [Register ("ClosetCollectionViewCell")]
    partial class ClosetCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ClothClassIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ClothClassLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ClothClassIcon != null) {
                ClothClassIcon.Dispose ();
                ClothClassIcon = null;
            }

            if (ClothClassLabel != null) {
                ClothClassLabel.Dispose ();
                ClothClassLabel = null;
            }
        }
    }
}