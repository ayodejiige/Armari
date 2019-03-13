using System;

using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using UIKit;
using Foundation;

namespace armari
{

    public partial class CalendarCollectionViewController : UICollectionViewController
    {
        public CalendarCollectionViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.ShowMessage("View Appeared");
            var source = CalendarCollectionView_.DataSource as CalendarCollectionSource;
            source.UpdateSource();
            CalendarCollectionView_.ReloadData();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            // Initialize message handler
            NSIndexPath indexPath = CalendarCollectionView_.GetIndexPathsForSelectedItems () [0];
            var source = CalendarCollectionView_.DataSource as CalendarCollectionSource;
            CalendarOutfit outfit = source.Outfits[(int)indexPath.Item];

            this.ShowMessage(string.Format("Showing Cloth -> {0}", (int)indexPath.Item));

            var dayViewController = segue.DestinationViewController as DayCollectionViewController;
            if (dayViewController != null)
            {
                DayCollectionViewController.outfit = outfit;
            } else
            {
                this.ShowAlert("Error" ,"Class failed to load");
            }
        }
    }


}

