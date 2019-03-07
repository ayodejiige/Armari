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

        public void Initialize(string label)
        {
            ClassNavigation.Title = label;
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

                SelectItemReq cloth;
                cloth.id = identifier;
                var location = Application.mh.ServiceInit<SelectItemReq>(cloth);

                var addViewController = segue.DestinationViewController as AddingItemViewController;
                if (addViewController != null)
                {

                }
                else
                {
                    this.ShowAlert("Error", "Class failed to load");
                }

            }
        }
    }
}

