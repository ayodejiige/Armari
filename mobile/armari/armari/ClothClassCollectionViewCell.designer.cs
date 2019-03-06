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
    [Register ("ClothClassCollectionViewCell")]
    partial class ClothClassCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ClothClassIcon { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ClothClassIcon != null) {
                ClothClassIcon.Dispose ();
                ClothClassIcon = null;
            }
        }
    }
}