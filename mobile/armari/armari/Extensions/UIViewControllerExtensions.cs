using System;

using UIKit;

namespace armari
{
    public static class UIViewControllerExtensions
    {
        public static void ShowAlert(this UIViewController self, string title, string message)
        {
            //Create Alert
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            //Add Action
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            // Present Alert
            self.PresentViewController(okAlertController, true, null);

            Console.WriteLine("ERROR: ARMARI -> {0}", message);
        }

        public static void ShowMessage(this UIViewController self, string message)
        {
            Console.WriteLine("({0}) INFO: ARMARI -> {1}", self.GetType().Name, message);
        }
    }
}
