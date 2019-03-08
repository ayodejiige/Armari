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

        //#region Overrides Methods
        //public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    // Always allow for highlighting
        //    return false;
        //}

        //public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    // Get cell and change to green background
        //    var cell = collectionView.CellForItem(indexPath);
        //    cell.ContentView.BackgroundColor = ArmariColors.B4957C;
        //}

        //public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    // Get cell and return to blue background
        //    var cell = collectionView.CellForItem(indexPath);
        //    cell.ContentView.BackgroundColor = ArmariColors.F1F1F2;
        //}
        //#endregion

    }
}
