using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace armari
{
    [Register("ClassCollectionView")]
    public class ClassCollectionView : UICollectionView
    {
        public string id;
        public List<int> Ids { get; set; } = new List<int>();

        #region Constructors
        public ClassCollectionView(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Override Methods
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            Application.logger.Message("Loaded class collection view");
            // Initialize
            DataSource = new ClassCollectionSource(this, ClosetCollectionViewController.CurrentClass);
            Delegate = new ClassCollectionDelegate(this);
        }
        #endregion

        public int IdentifierForIndexPath(NSIndexPath indexPath)
        {
            return Ids[(int)indexPath.Item];
        }
    }
}
