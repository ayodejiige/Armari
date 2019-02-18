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
        UIKit.UIButton AddButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView liveCameraStream { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TakePhotoButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton UploadButton { get; set; }

        [Action ("AddButtonTapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AddButtonTapped (UIKit.UIButton sender);

        [Action ("TakePhotoButtonTapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TakePhotoButtonTapped (UIKit.UIButton sender);

        [Action ("UploadButtonTapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UploadButtonTapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AddButton != null) {
                AddButton.Dispose ();
                AddButton = null;
            }

            if (liveCameraStream != null) {
                liveCameraStream.Dispose ();
                liveCameraStream = null;
            }

            if (TakePhotoButton != null) {
                TakePhotoButton.Dispose ();
                TakePhotoButton = null;
            }

            if (UploadButton != null) {
                UploadButton.Dispose ();
                UploadButton = null;
            }
        }
    }
}