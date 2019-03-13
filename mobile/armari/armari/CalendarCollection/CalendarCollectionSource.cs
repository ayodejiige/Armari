using System;
using Foundation;
using UIKit;
using System.Collections.Generic;


namespace armari
{
    public class CalendarCollectionSource : UICollectionViewDataSource
    {
        #region Computed Properties
        private readonly int MAX_CELLS = 6;
        public CalendarCollectionView CollectionView { get; set; }
        public List<CalendarOutfit> Outfits = new List<CalendarOutfit>();
        public List<int> Numbers { get; set; } = new List<int>();
        public List<string> ImagePaths { get; set; } = new List<string>();
        private readonly DateHandler CalendarDateHandler = new DateHandler();
        private List<string> Days { get; set; }
        #endregion

        #region Constructors
        public CalendarCollectionSource(CalendarCollectionView collectionView)
        {
            // Initialize
            CollectionView = collectionView;

            UpdateSource();

            //Days.Add(CalendarDateHandler.GetListOfDays(1)[0]);

            //ImagePaths.Add("outfitCollection_1.png");
            //ImagePaths.Add("outfitCollection_2.png");
            //ImagePaths.Add("item_5.png");
            //ImagePaths.Add("outfitIcon_1.png");

            // Init numbers collection
            //for (int n = 0; n < size; ++n)
            //{
            //    Numbers.Add(n);
    

                      //Numbers.Add(0);
        }
        #endregion

        public void UpdateSource()
        {
            Outfits = new List<CalendarOutfit>();
            int size = Application.UniversalCalentarLogger.MyCalendarOutfitContainer.Outfits.Count;
            //if (size > 6) size = 6;
            if (size == 0)
            {
                Application.UniversalCalentarLogger.AddOutfit(new CalendarOutfit());

            }
            
            if (Application.UniversalCalentarLogger.MyCalendarOutfitContainer.Outfits[0].CreatedDate != DateTime.Today) 
            {
                Application.UniversalCalentarLogger.AddOutfit(new CalendarOutfit());
            }

            size = Application.UniversalCalentarLogger.MyCalendarOutfitContainer.Outfits.Count;

            for (int n = 0; n < size; ++n)
            {
                //ImagePaths.Add(Application.UniversalCalentarLogger.MyCalendarOutfitContainer.Outfits[n].Top + ".png");
                //Days.Add(Application.UniversalCalentarLogger.MyCalendarOutfitContainer.Outfits[n].CreatedDate.ToString("D"));
                Outfits.Add(Application.UniversalCalentarLogger.MyCalendarOutfitContainer.Outfits[n]);
            }
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
            return Outfits.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get a reusable cell and set {~~it's~>its~~} title from the item
            var cell = collectionView.DequeueReusableCell("Cell", indexPath) as CalendarCollectionViewCell;
            //cell.Title = Numbers[(int)indexPath.Item].ToString();
            cell.Title = Outfits[(int)indexPath.Item].CreatedDate.ToString("D");
            //cell.Image = UIImage.FromBundle(ImagePaths[(int)indexPath.Item]);
            cell.Image = ClassIcons.NoOufitDay;
            //cell.CellImage.Layer.BorderWidth = 0;
            //cell.CellImage.Layer.CornerRadius = 20;
            //cell.CellImage.Layer.MasksToBounds = true;

            return cell;
        }

        public override bool CanMoveItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // We can always move items
            return false;
        }

        public override void MoveItem(UICollectionView collectionView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        {
            // Reorder our list of items
            var item = Numbers[(int)sourceIndexPath.Item];
            Numbers.RemoveAt((int)sourceIndexPath.Item);
            Numbers.Insert((int)destinationIndexPath.Item, item);
        }
        #endregion
    }
}