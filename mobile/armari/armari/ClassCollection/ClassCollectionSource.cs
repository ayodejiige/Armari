using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace armari
{

    public class ClassCollectionSource : UICollectionViewDataSource
    {
        #region Computed Properties
        public ClassCollectionView CollectionView { get; set; }
        public List<int> Classes { get; set; } = new List<int>();
        #endregion

        #region Constructors
        public ClassCollectionSource(ClassCollectionView collectionView, string type)
        {
            // Initialize
            CollectionView = collectionView;

            // Init numbers collection
            InitializeData(type);
        }
        #endregion

        private void InitializeData(String type)
        {
            Classes = Application.mh.GetWardrobe(type);
            string msg = "Items: \n";
            for (int n = 0; n < Classes.Count; ++n)
            {
                msg += string.Format("\t{0}\n", Classes[n]);
            }
            Application.logger.Message(msg);
        }

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
            var cell = collectionView.DequeueReusableCell("Cell", indexPath) as ClassCollectionViewCell;
            cell.Icon = ClassIcons.Icons["Tee"];

            return cell;
        }

        public override bool CanMoveItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // We can always move items
            return true;
        }

        #endregion
    }

}
