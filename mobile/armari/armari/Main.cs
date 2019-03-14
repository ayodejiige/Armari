using UIKit;
using System.IO;
using Foundation;
using System;

namespace armari
{

    public class Application
    {
        public static string USERID = "1";
        public static readonly Logger logger = Logger.Instance;
        public static MessageHandler mh = new MessageHandler("Armari2");
        public static CalendarLogger UniversalCalentarLogger = new CalendarLogger();
        public static string fileName = "armari_";
        public static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        // This is the main entry point of the application.
        static void Main(string[] args)
        {

            //save prev images
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var pics = Directory.EnumerateFiles("images/tmp");
            Console.WriteLine(String.Format("ARMARI -> Saving Pictures"));
            //foreach (var pic in pics)
            //{
            //    Console.WriteLine(String.Format("ARMARI -> Saving {0}", pic));
            //    //string filename = Path.Combine(pic);
            //    UIImage image_ = UIImage.FromFile(pic);
            //    string filename = Path.Combine(folderPath, pic.Split('/')[2]);
            //    NSData image = image_.AsPNG();
            //    NSError err = null;

            //    try
            //    {
            //        image.Save(filename, false, out err);
            //    }
            //    catch (Exception ex)
            //    {
            //        //logger.Error("File issue", ex.ToString());
            //    }
            //}


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
