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
    [Register ("AddingItemViewController")]
    partial class AddingItemViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Compartment1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Compartment2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Compartment3 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Compartment4 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView DisplayView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DoneButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Compartment1 != null) {
                Compartment1.Dispose ();
                Compartment1 = null;
            }

            if (Compartment2 != null) {
                Compartment2.Dispose ();
                Compartment2 = null;
            }

            if (Compartment3 != null) {
                Compartment3.Dispose ();
                Compartment3 = null;
            }

            if (Compartment4 != null) {
                Compartment4.Dispose ();
                Compartment4 = null;
            }

            if (DisplayView != null) {
                DisplayView.Dispose ();
                DisplayView = null;
            }

            if (DoneButton != null) {
                DoneButton.Dispose ();
                DoneButton = null;
            }
        }
    }
}