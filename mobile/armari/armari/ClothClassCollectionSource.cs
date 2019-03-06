using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace armari
{
    public class ClothClassCollectionSource : UICollectionViewDataSource
    {
        #region Computed Properties
        public ClothClassCollectionView CollectionView { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        #endregion

        #region Constructors
        public ClothClassCollectionSource(ClothClassCollectionView collectionView)
        {
            // Initialize
            CollectionView = collectionView;

            // Init numbers collection
            for (int n = 0; n < 100; ++n)
            {
                Classes.Add("coat");
            }
        }
        #endregion

        #region Override Methods
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            // We only have one section
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            // Return the number of items
            return Classes.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get a reusable cell and set {~~it's~>its~~} title from the item
            var cell = collectionView.DequeueReusableCell("Cell", indexPath) as ClothClassCollectionViewCell;
            cell.Icon = ClassIcons.Icons[Classes[(int)indexPath.Item]];

            return cell;
        }

        public override bool CanMoveItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // We can always move items
            return true;
        }

        //public override void MoveItem(UICollectionView collectionView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        //{
        //    // Reorder our list of items
        //    var item = Numbers[(int)sourceIndexPath.Item];
        //    Numbers.RemoveAt((int)sourceIndexPath.Item);
        //    Numbers.Insert((int)destinationIndexPath.Item, item);
        //}
        #endregion
    }

}
