using System;
using UIKit;

namespace armari
{
    public class Tools
    {
        public Tools()
        {
        }

        public static void ShowAlert(string title, string message)
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;

            //Create Alert
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            //Add Action
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            // Present Alert
            vc.PresentViewController(okAlertController, true, null);
        }
    }

    public struct ClothItem
    {
        public int id;
        public string itemType;
    }
}
