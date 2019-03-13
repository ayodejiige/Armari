using System;
using UIKit;

namespace armari
{
    public class Logger
    {
        public delegate double LogFunc(string msg);

        private static readonly Logger m_instance = new Logger();
        public event EventHandler<EventArgsT<String>> MessageUpdated = delegate { };
        public event EventHandler<EventArgsT<String>> ErrorOccurred = delegate { };

        static Logger()
        {
        }

        private Logger()
        {
        }

        public static Logger Instance
        {
            get
            {
                return m_instance;
            }
        }

        public void Message(string msg)
        {
            //MessageUpdated(this, new EventArgsT<string>(msg));
            string name = "";
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }
                name = vc.GetType().Name;
            });

            Console.WriteLine("{0} INFO: ARMARI -> {1}", name, msg);
        }

        public void Error(string title, string msg)
        {

            //ErrorOccurred(this, new EventArgsT<string>(msg));
            string name = "";
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }
                //Create Alert
                var okAlertController = UIAlertController.Create(title, msg, UIAlertControllerStyle.Alert);

                //Add Action
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Destructive, null));

                // Present Alert
                vc.PresentViewController(okAlertController, true, null);

                name = vc.GetType().Name;
            });

            Console.WriteLine("{0} ERROR: ARMARI -> {1}", name, msg);
        }

    }
}