using UIKit;
using System.IO;
using Foundation;

namespace armari
{

    public class Application
    {
        public static string USERID = "1";
        public static readonly Logger logger = Logger.Instance;
        public static MessageHandler mh = new MessageHandler("Armari2");

        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            mh.Init();
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UITextAttributes textAttrib = new UITextAttributes();
            textAttrib.TextColor = ArmariColors.FEC821;
            UITabBarItem.Appearance.SetTitleTextAttributes(textAttrib, UIControlState.Selected);
            UIBarButtonItem.Appearance.SetTitleTextAttributes(textAttrib, UIControlState.Normal);
            UINavigationBar.Appearance.SetTitleTextAttributes(textAttrib);
            UIButton.Appearance.SetTitleColor(ArmariColors.FEC821, UIControlState.Normal);
            UIWindow.Appearance.TintColor = ArmariColors.FEC821;
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
