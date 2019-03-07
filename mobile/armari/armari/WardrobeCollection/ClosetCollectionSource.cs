using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace armari
{
    public class ClosetCollectionSource : UICollectionViewDataSource
    {
        #region Computed Properties
        public ClosetCollectionView CollectionView { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        #endregion

        #region Constructors
        public ClosetCollectionSource(ClosetCollectionView collectionView)
        {
            // Initialize
            CollectionView = collectionView;
            CollectionView.Classes = Classes;
            // Init numbers collection
            foreach (string key in ClassIcons.Icons.Keys)
            {
                Classes.Add(key);
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
            var cell = collectionView.DequeueReusableCell("Cell", indexPath) as ClosetCollectionViewCell;
            cell.Label = Classes[(int)indexPath.Item];
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

        public string IdentifierForIndexPath(NSIndexPath indexPath)
        {
            return Classes[(int)indexPath.Item];
        }
    }

}
