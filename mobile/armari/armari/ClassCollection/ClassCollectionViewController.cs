using System;
using UIKit;
using Foundation;
using System.Threading.Tasks;

namespace armari
{
    public enum ClassCollectionType
    {
        OutfitSelection,
        OutfitRet
    }

    public partial class ClassCollectionViewController : UICollectionViewController
    {
        public string label;
        public ClassCollectionType classCollectionType;
        public int outfitCategory;

        public ClassCollectionViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            this.ShowMessage("View Loaded");
            ClassCollectionView_.AllowsMultipleSelection = false;
            ClassCollectionView_.PrefetchingEnabled = false;

        }

        public async override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.ShowMessage("View appeared");

            this.StartLoadingOverlay();

            await Task.Factory.StartNew(() => {
                ClassCollectionView_.InitializeData(label);
            });

            ClassCollectionView_.UpdateDataSource();
            ClassCollectionView_.ReloadData();

            var delegate_ = ClassCollectionView_.Delegate as ClassCollectionDelegate;
            delegate_.Controller = this;

            var source_ = ClassCollectionView_.DataSource as ClassCollectionSource;
            if(source_.Ids.Count == 0 & classCollectionType == ClassCollectionType.OutfitSelection)
            {
                RetreiveButton.Hidden = true;
            }
            else if(classCollectionType == ClassCollectionType.OutfitSelection)
            {
                string id = "-1";
                switch (outfitCategory)
                {
                    case 0:
                        id = DayCollectionViewController.outfit.Layer;
                        break;
                    case 1:
                        id = DayCollectionViewController.outfit.Top;
                        break;
                    case 3:
                        id = DayCollectionViewController.outfit.Bottom;
                        break;
                    case 4:
                        id = DayCollectionViewController.outfit.Footwear;
                        break;
                    default:
                        break;
                }
                int idAsInt = int.Parse(id);
                var index = source_.Ids.FindIndex(x => x == idAsInt);
                this.ShowMessage(string.Format("Item {0} at index {1}", idAsInt, index));
                if(index == -1)
                {
                    RetreiveButton.Hidden = true;
                }
                else
                {
                    NSIndexPath nip = NSIndexPath.FromItemSection((int)index, 0);
                    ClassCollectionView_.SelectItem(nip, false, UICollectionViewScrollPosition.None);
                    //ClassCollectionView_.Delegate.ItemSelected(ClassCollectionView_, nip);

                    RetreiveButton.Hidden = false;
                }
                //NSIndexPath nip = NSIndexPath.FromItemSection((int), 0);
                //cvColor.SelectItem(nip, false, UICollectionViewScrollPosition.Bottom);
            }

            this.StopLoadingOverlay();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            //NSIndexPath nip = ClassCollectionView_.GetIndexPathsForSelectedItems()[0];
            //ClassCollectionView_.Delegate.ItemSelected(ClassCollectionView_, nip);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void Initialize(string label_)
        {
            label = label_;
            ClassNavigation.Title = label_;
            if(label_ == "Dangling")
            {
                ClassNavigation.Title = "To Return";
            }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "RetrieveSegue")
            {
                // Initialize message handler
                NSIndexPath indexPath = ClassCollectionView_.GetIndexPathsForSelectedItems()[0];
                int identifier = ClassCollectionView_.IdentifierForIndexPath(indexPath);
                this.ShowMessage(string.Format("RetrieveSegue Cloth -> {0}", identifier));

                var retItemController = segue.DestinationViewController as AddingItemViewController;
                if (retItemController != null)
                {
                    retItemController.retId = identifier;
                    retItemController.retCategory = label;
                    retItemController.identifier = "ret";
                }
                else
                {
                    this.ShowAlert("Error", "Class failed to load");
                }

            } else if (segue.Identifier == "RetrieveDayItemSegue")
            {
                // Initialize message handler
                NSIndexPath indexPath = ClassCollectionView_.GetIndexPathsForSelectedItems()[0];
                int identifier = ClassCollectionView_.IdentifierForIndexPath(indexPath);
                this.ShowMessage(string.Format("RetrieveSegue Cloth -> {0}", identifier));

                var retItemController = segue.DestinationViewController as AddingItemViewController;
                if (retItemController != null)
                {
                    retItemController.retId = identifier;
                    retItemController.retCategory = label;
                    retItemController.identifier = "day";
                }
                else
                {
                    this.ShowAlert("Error", "Class failed to load");
                }

            }
        }
    }
}

