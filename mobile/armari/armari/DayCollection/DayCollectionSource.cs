using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.IO;



namespace armari
{
    public class DayCollectionSource : UICollectionViewDataSource
    {
        #region Computed Properties
        //public DayCollectionView CollectionView { get; set; }
        public List<string> Ids { get; set; } = new List<string>();
        public List<string> Titles { get; set; } = new List<string>();
        public List<string> ImagePaths { get; set; } = new List<string>();
        private readonly DateHandler DayDateHandler = new DateHandler();
        private List<string> Days { get; set; }
        #endregion

        #region Constructors
        public DayCollectionSource(DayCollectionView collectionView)
        {
            // Initialize
            //CollectionView = collectionView;
            Titles.Add("Layer");
            Titles.Add("Top");
            Titles.Add("Bottom");
            Titles.Add("Footwear");

            UpdateSource();

        }
        #endregion

        public void UpdateSource()
        {
            Ids = new List<string>();
            var outfit = DayCollectionViewController.outfit;
            Ids.Add(outfit.Layer);     
            Ids.Add(outfit.Top);
            Ids.Add(outfit.Bottom);
            Ids.Add(outfit.Footwear);
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
            var cell = collectionView.DequeueReusableCell("Cell", indexPath) as DayCollectionViewCell;
            Application.logger.Message(string.Format("Loading cell {0} -> {1}", indexPath.Item, Ids[(int)indexPath.Item]));
            cell.Title = Titles[(int)indexPath.Item];
            string filename = Path.Combine(Application.folderPath, Application.fileName + Ids[(int)indexPath.Item] + ".png");
            UIImage image = UIImage.FromFile(filename);
            if(image == null)
            {
                Application.logger.Message("No image");
                cell.Image = ClassIcons.NoOufitDay;
            }
            else
            {
                Application.logger.Message("Yes image");
                cell.Image = image.CenterCrop();
            }

            cell.CornerRadius = 20;
            cell.MasksToBounds = true;
            cell.BorderWidth = 0;
            cell.BorderColor = UIColor.Clear;

            return cell;
        }

        public override bool CanMoveItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // We can always move items
            return false;
        }

        //public override void MoveItem(UICollectionView collectionView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        //{
        //    // Reorder our list of items
        //    var item = Numbers[(int)sourceIndexPath.Item];
        //    //Numbers.RemoveAt((int)sourceIndexPath.Item);
        //    Numbers.Insert((int)destinationIndexPath.Item, item);
        //}
        #endregion
    }
}