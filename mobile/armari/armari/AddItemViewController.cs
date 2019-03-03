using System;

using UIKit;

namespace armari
{
    public partial class AddItemViewController : UIViewController
    {
        public UIImage Image { get; set; }
        private Logger logger;

        protected AddItemViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            // Setup Logger
            logger = Logger.Instance;
            logger.ErrorOccurred += (s, e) => this.ShowAlert("Processing Error", e.Value);
            logger.MessageUpdated += (s, e) => this.ShowMessage(e.Value);
            logger.Message("View Loaded");

            // UI Setup
            if (ImageView.Image != null)
            {
                ImageView.Image.Dispose();
            }
            ImageView.Image = Image;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

