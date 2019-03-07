using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace armari
{
    [Register("ClosetCollectionView")]
    public class ClosetCollectionView : UICollectionView
    {
        public List<string> Classes { get; set; } = new List<string>();
        #region Constructors
        public ClosetCollectionView(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Override Methods
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            // Initialize
            DataSource = new ClosetCollectionSource(this);
            Delegate = new ClosetCollectionDelegate(this);

        }
        #endregion

    public string IdentifierForIndexPath(NSIndexPath indexPath)
    {
        return Classes[(int)indexPath.Item];
    }

    }
}
