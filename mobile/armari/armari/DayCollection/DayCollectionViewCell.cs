using System;
using CoreAnimation;
using Foundation;
using UIKit;

namespace armari
{
    public partial class DayCollectionViewCell : UICollectionViewCell
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

        public nfloat BorderWidth
        {
            set { CellImage.Layer.BorderWidth = value; }
        }

        public bool MasksToBounds
        {
            set { CellImage.Layer.MasksToBounds = value; }
        }

        public UIColor BorderColor
        {
            set { CellImage.Layer.BorderColor = value.CGColor; }
        }

        public nfloat CornerRadius
        {
            set { CellImage.Layer.CornerRadius = value; }
        }
        #endregion

        public DayCollectionViewCell(IntPtr handle) : base(handle)
        {
        }

        public override void DisplayLayer(CALayer layer)
        {
            base.DisplayLayer(layer);
        }

        static DayCollectionViewCell()
        {
            Nib = UINib.FromName("MyCollectionViewCell", NSBundle.MainBundle);
        }
    }
}
