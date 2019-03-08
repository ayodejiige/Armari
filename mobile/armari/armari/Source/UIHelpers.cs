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
            ["Blouse"] = UIImage.FromFile("images/classes/blouse icon.png"),
            ["Cardigan"] = UIImage.FromFile("images/classes/cardigan icon.png"),
            ["Hoodie"] = UIImage.FromFile("images/classes/hodie icon.jpg"),
            ["Jackets"] = UIImage.FromFile("images/classes/jacket icon.png"),
            ["Jeans"] = UIImage.FromFile("images/classes/jeans.png"),
            ["Shorts"] = UIImage.FromFile("images/classes/shorts icon.png"),
            ["Skirt"] = UIImage.FromFile("images/classes/skirt icon.png"),
            ["Sweater"] = UIImage.FromFile("images/classes/sweater icon.png"),
            ["Tee"] = UIImage.FromFile("images/classes/tee Icon.jpg"),
            ["Dangling"] = UIImage.FromFile("images/classes/return icon 2.png")
        };
    }
}