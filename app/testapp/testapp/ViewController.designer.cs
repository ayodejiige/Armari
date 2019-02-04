// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace testapp
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView liveCameraStream { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TranslateButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (liveCameraStream != null) {
                liveCameraStream.Dispose ();
                liveCameraStream = null;
            }

            if (TranslateButton != null) {
                TranslateButton.Dispose ();
                TranslateButton = null;
            }
        }
    }
}