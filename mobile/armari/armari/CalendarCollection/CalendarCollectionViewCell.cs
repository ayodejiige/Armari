using System;
using CoreAnimation;
using Foundation;
using UIKit;

namespace armari
{
    public partial class CalendarCollectionViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("MyCollectionViewCell");
        public static readonly UINib Nib;

        #region Computed Properties
        public string Title
        {
            get { return TextLabel.Text; }
            set { TextLabel.Text = value; }
        }

        public UIImage Image
        {
            get { return CellImage.Image; }
            set { CellImage.Image = value; }
        }

        #endregion

        public CalendarCollectionViewCell(IntPtr handle) : base(handle)
        {
        }

        public override void DisplayLayer(CALayer layer)
        {
            base.DisplayLayer(layer);
            CellImage.Layer.BorderWidth = 0;
            CellImage.Layer.CornerRadius = 20;
            CellImage.Layer.MasksToBounds = true;
        }

        static CalendarCollectionViewCell()
        {
            Nib = UINib.FromName("MyCollectionViewCell", NSBundle.MainBundle);
        }
    }
}
