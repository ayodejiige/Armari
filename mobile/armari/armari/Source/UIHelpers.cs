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
        public static UIColor F1F1F2 = UIColor.FromRGB(0.95f, 0.95f, 0.90f);
        public static UIColor B4957C = UIColor.FromRGB(0.71f, 0.58f, 0.49f);
        public static UIColor FEC821 = UIColor.FromRGB(1.00f, 0.78f, 0.13f);
        public static UIColor EDA31D = UIColor.FromRGB(0.93f, 0.64f, 0.11f);
        public static UIColor E6D1C8 = UIColor.FromRGB(0.90f, 0.82f, 0.78f);
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
            ["Footwear"] = UIImage.FromFile("images/classes/tee Icon.jpg"),
            ["Dangling"] = UIImage.FromFile("images/classes/return icon 2.png")
        };
        public static UIImage ClosetUnselected = UIImage.FromFile("images/closet_unselected.png");
        public static UIImage ClosetSelected = UIImage.FromFile("images/closet_selected.png");
        public static UIImage NoOufitDay = UIImage.FromFile("images/outfitIcon_1.png");
    }
}