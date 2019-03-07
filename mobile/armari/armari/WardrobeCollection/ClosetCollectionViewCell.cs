using System;

using Foundation;
using UIKit;

namespace armari
{
    public partial class ClosetCollectionViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("ClosetCollectionViewCell");
        public static readonly UINib Nib;
        public UIImage Icon
        {
            get { return ClothClassIcon.Image; }
            set { ClothClassIcon.Image = value; }
        }

        public String Label
        {
            get { return ClothClassLabel.Text; }
            set { ClothClassLabel.Text = value; }
        }

        static ClosetCollectionViewCell()
        {
            Nib = UINib.FromName("ClosetCollectionViewCell", NSBundle.MainBundle);
        }

        protected ClosetCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

        }

    }
}
