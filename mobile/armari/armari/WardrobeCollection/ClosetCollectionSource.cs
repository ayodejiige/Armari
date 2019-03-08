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
            string label = Classes[(int)indexPath.Item];
            if (label == "Dangling")
            {
                cell.Label = "To Return";
            }
            else
            {
                cell.Label = label;
            }
            cell.Icon = ClassIcons.Icons[Classes[(int)indexPath.Item]];

            return cell;
        }

        public override bool CanMoveItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // We can always move items
            return true;
        }

        #endregion

        public string IdentifierForIndexPath(NSIndexPath indexPath)
        {
            return Classes[(int)indexPath.Item];
        }
    }

}
