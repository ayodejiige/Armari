﻿using System;

using Foundation;
using UIKit;

namespace armari
{
    public partial class ClothClassCollectionViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("ClothClassCollectionViewCell");
        public static readonly UINib Nib;
        public UIImage Icon
        {
            get { return ClothClassIcon.Image; }
            set { ClothClassIcon.Image = value; }
        }

        static ClothClassCollectionViewCell()
        {
            Nib = UINib.FromName("ClothClassCollectionViewCell", NSBundle.MainBundle);
        }

        protected ClothClassCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
