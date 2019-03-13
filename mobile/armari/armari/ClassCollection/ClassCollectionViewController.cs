using System;
using UIKit;
using Foundation;
using System.Threading.Tasks;

namespace armari
{
    public enum ClassCollectionType
    {
        OutfitSelection
    }

    public partial class ClassCollectionViewController : UICollectionViewController
    {
        private Logger logger;
        public string label;
        public ClassCollectionType classCollectionType;

        public ClassCollectionViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            logger = Logger.Instance;
            logger.ErrorOccurred += (s, e) => this.ShowAlert("Processing Error", e.Value);
            logger.MessageUpdated += (s, e) => this.ShowMessage(e.Value);
            this.ShowMessage("View Loaded");
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

            this.StopLoadingOverlay();
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

            }
        }
    }
}

