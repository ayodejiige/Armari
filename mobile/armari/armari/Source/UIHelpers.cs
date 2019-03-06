using System;
using UIKit;
using System.Collections.Generic;

namespace armari
{
    public class UIHelpers
    {

        public UIHelpers()
        {
        }
    }

    public static class ArmariColors
    {
        public static UIColor F1F1F2 = UIColor.FromRGB((float)0.95, (float)0.95, (float)0.9);
        public static UIColor B4957C = UIColor.FromRGB((float)0.71, (float)0.58, (float)0.49);
    }

    public static class ClassIcons
    {
        public static Dictionary<string, UIImage> Icons = new Dictionary<string, UIImage>()
        {
            ["coat"] = UIImage.FromFile("images/classes/coat.png")
        };
    }
}
