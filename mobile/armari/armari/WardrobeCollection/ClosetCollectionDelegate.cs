using System;
using UIKit;
using Foundation;

namespace armari
{

    public class ClosetCollectionDelegate : UICollectionViewDelegate
    {
        //WeakReference<DashboardViewController>  _dvc;
        #region Computed Properties
        public ClosetCollectionView CollectionView { get; set; }
        #endregion




        #region Constructors
        public ClosetCollectionDelegate(ClosetCollectionView collectionView)
        {

            // Initialize
            CollectionView = collectionView;

        }
        #endregion

    }
}
