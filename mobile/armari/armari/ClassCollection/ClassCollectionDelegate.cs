using System;
using UIKit;
using Foundation;

namespace armari
{
    public class ClassCollectionDelegate : UICollectionViewDelegate
    {
        #region Computed Properties
        public ClassCollectionViewController Controller;
        public ClassCollectionView CollectionView { get; set; }
        #endregion

        #region Constructors
        public ClassCollectionDelegate(ClassCollectionView collectionView)
        {

            // Initialize
            CollectionView = collectionView;

        }
        #endregion

        #region Overrides Methods
        //public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    // Always allow for highlighting
        //    return true;
        //}
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            //base.ItemSelected(collectionView, indexPath);
            Application.logger.Message(string.Format("{0} Selected", indexPath.Item));

            if (Controller.classCollectionType == ClassCollectionType.OutfitSelection)
            {
                // Logic to update logger based on selected item
                var source_ = CollectionView.DataSource as ClassCollectionSource;
                string item = source_.Ids[(int)indexPath.Item].ToString();
                switch (indexPath.Item)
                {
                    case 0:
                        DayCollectionViewController.outfit.Layer = item;
                        break;
                    case 1:
                        DayCollectionViewController.outfit.Top = item;
                        break;
                    case 3:
                        DayCollectionViewController.outfit.Bottom = item;
                        break;
                    case 4:
                        DayCollectionViewController.outfit.Footwear = item;
                        break;
                    default:
                        break;
                }
                Application.UniversalCalentarLogger.AddOutfit(DayCollectionViewController.outfit);

                // Pop back to previous view
                // Controller.NavigationController.PopViewController(true);
                var cell = CollectionView.CellForItem(indexPath);
                cell.ContentView.BackgroundColor = ArmariColors.EDA31D;
            }
        }

        public override void ItemDeselected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            //base.ItemDeselected(collectionView, indexPath);
            Application.logger.Message(string.Format("{0} De-Selected", indexPath.Item));
            if (Controller.classCollectionType == ClassCollectionType.OutfitSelection)
            {
                // Logic to update logger based on selected item

                // Pop back to previous view
                // Controller.NavigationController.PopViewController(true);
                var cell = CollectionView.CellForItem(indexPath);
                cell.ContentView.BackgroundColor = ArmariColors.F1F1F2;
            }
        }

        //private void HandleSelection()
        //{
        //    if (Controller.classCollectionType == ClassCollectionType.OutfitSelection)
        //    {
        //        // Logic to update logger based on selected item

        //        // Pop back to previous view
        //        // Controller.NavigationController.PopViewController(true);
        //        var cell = CollectionView.CellForItem(indexPath);
        //        cell.ContentView.BackgroundColor = ArmariColors.EDA31D;
        //    }
        //}

        //private void HandleDeselection()
        //{
        //    if (Controller.classCollectionType == ClassCollectionType.OutfitSelection)
        //    {
        //        // Logic to update logger based on selected item

        //        // Pop back to previous view
        //        // Controller.NavigationController.PopViewController(true);

        //    }
        //}


        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {

            // Get cell and change to green background
            //var cell = collectionView.CellForItem(indexPath);
            //cell.ContentView.BackgroundColor = ArmariColors.B4957C;
        }

        public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get cell and return to blue background
            //var cell = collectionView.CellForItem(indexPath);
            //cell.ContentView.BackgroundColor = ArmariColors.B4957C;
        }
        #endregion
    }
}
