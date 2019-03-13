using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Threading.Tasks;

namespace armari
{
    public partial class ClosetCollectionViewController : UICollectionViewController
    {
        public static string CurrentClass {get;set;}
        public static List<int> Ids { get; set; } = null;

        private Logger logger;
        public ClosetCollectionViewController(IntPtr handle) : base(handle)
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

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
        }

        public async override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            //this.ShowMessage("View appeared");

            //this.StartLoadingOverlay();

            //await Task.Factory.StartNew(() => {
            //    var wardrobe = Application.mh.GetWardrobeAll();
            //});

            //this.StopLoadingOverlay();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if(segue.Identifier == "ClassSegue")
            {
                // Initialize message handler
                NSIndexPath indexPath = ClosetCollectionView_.GetIndexPathsForSelectedItems () [0];
                string identifier = ClosetCollectionView_.IdentifierForIndexPath(indexPath);
                this.ShowMessage(string.Format("Showing Cloth -> {0}", (int)indexPath.Item));

                var classViewController = segue.DestinationViewController as ClassCollectionViewController;
                if (classViewController != null)
                {
                    classViewController.Initialize(identifier);
                    //var cv = classViewController.CollectionView as ClassCollectionView;
                    //this.StartLoadingOverlay();
                    //Ids = ClassCollectionView.InitializeDataStatic(identifier);

                    CurrentClass = identifier;
                } else
                {
                    this.ShowAlert("Error" ,"Class failed to load");
                }
            }

        }
    }
}

