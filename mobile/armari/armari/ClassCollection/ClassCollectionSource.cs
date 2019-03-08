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
        public ClassCollectionView CollectionView { get; set; }
        public List<int> Ids { get; set; } = new List<int>();
        #endregion

        private static string fileName = "amari_";
        private static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        #region Constructors
        public ClassCollectionSource(ClassCollectionView collectionView, string type)
        {
            // Initialize
            CollectionView = collectionView;

            // Init numbers collection
            InitializeData(type);
            CollectionView.Ids = Ids;
        }
        #endregion

        private void InitializeData(String type)
        {
            Ids = Application.mh.GetWardrobe(type);
            string msg = "Items: \n";
            for (int n = 0; n < Ids.Count; ++n)
            {
                msg += string.Format("\t{0}\n", Ids[n]);
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
            return Ids.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get a reusable cell and set {~~it's~>its~~} title from the item
            var cell = collectionView.DequeueReusableCell("Cell", indexPath) as ClassCollectionViewCell;
            string filename = Path.Combine(folderPath, fileName + Ids[(int)indexPath.Item] + ".png");
            cell.Icon = UIImage.FromFile(filename);

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
