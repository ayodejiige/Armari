using System;
using UIKit;
using System.Collections.Generic;
using Foundation;


namespace armari
{
    [Register("DayCollectionView")]
    public class DayCollectionView : UICollectionView
    {
        #region Constructors
        public DayCollectionView(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Override Methods
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            // Initialize
            DataSource = new DayCollectionSource(this);
            Delegate = new DayCollectionDelegate(this);

        }
        #endregion
    }
}
