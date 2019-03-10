using System;
using UIKit;
using Foundation;

namespace armari
{
    public partial class ClassCollectionViewController : UICollectionViewController
    {
        private Logger logger;
        public string label;

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

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void Initialize(string label_)
        {
            label = label_;
            ClassNavigation.Title = label_;
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
                Location location = new Location();

                if(label == "Dangling")
                {
                    this.ShowMessage("Returning Item");
                    ReturnItemReq cloth;
                    cloth.id = identifier;
                    location = Application.mh.ServiceInit<ReturnItemReq>(cloth);
                }
                else
                {
                    this.ShowMessage("Selecting Item");
                    SelectItemReq cloth;
                    cloth.id = identifier;
                    location = Application.mh.ServiceInit<SelectItemReq>(cloth);
                }

                if (location.locs == null)
                {
                    this.ShowAlert("Location Error", "Got no location from closet");
                    this.NavigationController.PopViewController(true);
                    //return;
                }

                var retItemController = segue.DestinationViewController as AddingItemViewController;
                if (retItemController != null)
                {
                    retItemController.identifier = "ret";
                    retItemController.location = location;
                }
                else
                {
                    this.ShowAlert("Error", "Class failed to load");
                }

            }
        }
    }
}

