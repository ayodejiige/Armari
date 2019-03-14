using System;

using Foundation;
using UIKit;
using CoreGraphics;

namespace armari
{
    public partial class ClassCollectionViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("ClassCollectionViewCell");
        public static readonly UINib Nib;
        public UIImage Icon
        {
            get { return ClothItemImage.Image; }
            set { ClothItemImage.Image = value; }
        }

        public nfloat BorderWidth
        {
            set { ClothItemImage.Layer.BorderWidth = value; }
        }

        public bool MasksToBounds
        {
            set { ClothItemImage.Layer.MasksToBounds = value; }
        }

        public UIColor BorderColor
        {
            set { ClothItemImage.Layer.BorderColor = value.CGColor; }
        }

        public nfloat CornerRadius
        {
            set { ClothItemImage.Layer.CornerRadius = value; }
        }


        static ClassCollectionViewCell()
        {
            Nib = UINib.FromName("ClassCollectionViewCell", NSBundle.MainBundle);
        }

        protected ClassCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
