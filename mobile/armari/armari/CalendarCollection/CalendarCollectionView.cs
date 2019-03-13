using System;
using UIKit;
using System.Collections.Generic;
using Foundation;


namespace armari
{
    [Register("CalendarCollectionView")]

    public class CalendarCollectionView : UICollectionView
    {
        //public CalendarCollectionSource Source;
        #region Constructors
        public CalendarCollectionView(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Override Methods
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            // Initialize
            DataSource = new CalendarCollectionSource(this);
            Delegate = new CalendarCollectionDelegate(this);
            //Source = DataSource as CalendarCollectionSource;
        }
        #endregion

    }
}
