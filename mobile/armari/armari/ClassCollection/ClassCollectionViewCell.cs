using System;

using Foundation;
using UIKit;

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
