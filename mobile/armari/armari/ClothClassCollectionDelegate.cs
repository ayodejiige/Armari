using System;
using Foundation;
using UIKit;

namespace armari
{
    public class ClothClassCollectionDelegate : UICollectionViewDelegate
    {
        #region Computed Properties
        public ClothClassCollectionView CollectionView { get; set; }
        #endregion

        #region Constructors
        public ClothClassCollectionDelegate(ClothClassCollectionView collectionView)
        {

            // Initialize
            CollectionView = collectionView;

        }
        #endregion

        #region Overrides Methods
        public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Always allow for highlighting
            return true;
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get cell and change to green background
            var cell = collectionView.CellForItem(indexPath);
            cell.ContentView.BackgroundColor = ArmariColors.B4957C;
        }

        public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get cell and return to blue background
            var cell = collectionView.CellForItem(indexPath);
            cell.ContentView.BackgroundColor = ArmariColors.B4957C;
        }
        #endregion
    }
}
