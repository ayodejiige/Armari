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
    [Register ("AddItemViewController")]
    partial class AddItemViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView AddItemView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView ClassPicker { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ClothClassLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddItemView != null) {
                AddItemView.Dispose ();
                AddItemView = null;
            }

            if (ClassPicker != null) {
                ClassPicker.Dispose ();
                ClassPicker = null;
            }

            if (ClothClassLabel != null) {
                ClothClassLabel.Dispose ();
                ClothClassLabel = null;
            }

            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }
        }
    }
}