using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace armari
{
    [Register("ClothClassCollectionView")]
    public class ClothClassCollectionView : UICollectionView
    {

        #region Constructors
        public ClothClassCollectionView(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Override Methods
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            // Initialize
            DataSource = new ClothClassCollectionSource(this);
            Delegate = new ClothClassCollectionDelegate(this);

        }
        #endregion
    }
}
