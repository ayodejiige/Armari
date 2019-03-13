using System;
using CoreGraphics;
using UIKit;
using Foundation;

namespace armari
{
    public static class UIViewControllerExtensions
    {
        //private static Logger logger;
        //public static UIAlertView AlertView { get; set; }
        public static UIAlertController loadingAlertController;
        public static UIView overlay;
        public static void ShowAlert(this UIViewController self, string title, string message)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                //Create Alert
                var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

                //Add Action
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Destructive, null));

                // Present Alert
                self.PresentViewController(okAlertController, true, null);
            });
            Console.WriteLine("ERROR: ARMARI -> {0}", message);
        }

        public static void ShowMessage(this UIViewController self, string message)
        {
            Console.WriteLine("({0}) INFO: ARMARI -> {1}", self.GetType().Name, message);
        }

        public static void StartLoadingOverlay(this UIViewController self)
        {
            //Create Alert
            loadingAlertController = UIAlertController.Create("", "Please wait...", UIAlertControllerStyle.Alert);
            UIActivityIndicatorView loadingIndicator = new UIActivityIndicatorView(new CGRect(10, 5, 50, 50));
            loadingIndicator.HidesWhenStopped = true;
            loadingIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
            loadingIndicator.StartAnimating();

            //self.View.AddSubview(loadingIndicator);
            loadingAlertController.View.AddSubview(loadingIndicator);
            //Add Action
            //okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            // Present Alert
            self.PresentViewController(loadingAlertController, false, null);
        }

        public static void StopLoadingOverlay(this UIViewController self)
        {
            loadingAlertController.DismissViewController(false, null);
        }

        public static void AddLoadingOverlay(this UIViewController self)
        {
            //overlay = new UIView();
            //overlay.BackgroundColor = ArmariColors.B4957C;
            //overlay.Alpha = (float)0.5;
            //overlay.Frame = UIScreen.MainScreen.Bounds;
            //overlay.Hidden = true;
            //self.View.AddSubview(overlay);
        }

        public static void ShowLoadingOverlay(this UIViewController self)
        {
            //overlay.Hidden = false;
        }

        public static void HideLoadingOverlay(this UIViewController self)
        {
            //overlay.Hidden = true;
        }
    }
}
