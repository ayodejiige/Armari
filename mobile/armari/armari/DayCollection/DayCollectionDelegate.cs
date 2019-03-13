using System;
using Foundation;
using UIKit;
using System.Collections.Generic;

namespace armari
{
    public class DayCollectionDelegate : UICollectionViewDelegate
    {
        #region Computed Properties
        public DayCollectionView CollectionView { get; set; }
        #endregion

        #region Constructors
        public DayCollectionDelegate(DayCollectionView collectionView)
        {

            // Initialize
            CollectionView = collectionView;

        }
        #endregion

        #region Overrides Methods
        //public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    base.ItemSelected(collectionView, indexPath);
        //    Application.logger.Message(string.Format("{0} SELECTED", indexPath.Item));
        //}

        //public override void ItemDeselected(UICollectionView collectionView, NSIndexPath indexPath)
        //{
        //    base.ItemDeselected(collectionView, indexPath);
        //    Application.logger.Message(string.Format("{0} DE-SELECTED", indexPath.Item));
        //}

        public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }


            return true;
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get cell and change to green background
            //var cell = collectionView.CellForItem(indexPath);
            //cell.ContentView.BackgroundColor = UIColor.FromRGB(183, 208, 57);
        }

        public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get cell and return to blue background
            //var cell = collectionView.CellForItem(indexPath);
            //cell.ContentView.BackgroundColor = UIColor.FromRGB(164, 205, 255);
        }
        #endregion
    }
}
