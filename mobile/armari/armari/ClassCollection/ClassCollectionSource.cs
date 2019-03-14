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
            Application.logger.Message(string.Format("Loading cell {0} -> {1}", indexPath.Item, Ids[(int)indexPath.Item]));
            var cell = collectionView.DequeueReusableCell("Cell", indexPath) as ClassCollectionViewCell;
            string filename = Path.Combine(Application.folderPath, Application.fileName + Ids[(int)indexPath.Item] + ".png");
            UIImage image = UIImage.FromFile(filename);
            if (image == null)
            {
                cell.Icon = ClassIcons.Icons["Shorts"];
            }
            else
            {
                cell.Icon = image.CenterCrop();
            }

            cell.CornerRadius = 20;
            cell.MasksToBounds = true;
            cell.BorderWidth = 5;
            cell.BorderColor = UIColor.Clear;

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
