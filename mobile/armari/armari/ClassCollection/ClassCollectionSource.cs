using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using System.IO;

namespace armari
{

    public class ClassCollectionSource : UICollectionViewDataSource
    {
        #region Computed Properties
        private ClassCollectionView CollectionView { get; set; }
        public List<int> Ids { get; set; } = null;
        #endregion

        private static string fileName = "armari_";
        private static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        #region Constructors
        public ClassCollectionSource(ClassCollectionView collectionView)
        {
            // Initialize
            CollectionView = collectionView;
            Ids = CollectionView.Ids;
        }
        #endregion

        public void SetIds(List<int> ids)
        {
            Ids = ids;
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
            return Ids.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get a reusable cell and set {~~it's~>its~~} title from the item
            var cell = collectionView.DequeueReusableCell("Cell", indexPath) as ClassCollectionViewCell;
            string filename = Path.Combine(folderPath, fileName + Ids[(int)indexPath.Item] + ".png");
            UIImage image = UIImage.FromFile(filename);
            if (image == null)
            {
                cell.Icon = ClassIcons.Icons["Shorts"];
            }
            else
            {
                cell.Icon = UIImage.FromFile(filename);
            }

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
