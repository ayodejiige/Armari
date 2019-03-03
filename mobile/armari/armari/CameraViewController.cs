using System;
using Foundation;
using UIKit;

namespace armari
{
    public partial class CameraViewController : UIViewController
    {
        private CameraController cam;
        private MessageHandler mh;
        private Classifier classifier;
        private Logger logger;

        protected CameraViewController(IntPtr handle) : base(handle)
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

            // Setup Camera
            cam = new CameraController();
            cam.ImagePicked += (s, e) => PhotoSelected(e.Value);

            // UI Setup
            //CameraView.BackgroundColor = ArmariColors.F1F1F2;

            //Button Callbacks

            //Start Camera
            var picker = cam.ShowCamera();
            PresentViewController(picker, true, null);
        }

        void AddItemButton_TouchUpInside(object sender, EventArgs e)
        {
            // Run ML model
            //CGSize size = new CGSize(224, 224);
            //UIImage imageSize = image.Scale(size);
            //Prediction prediction = classifier.Classify(image);
            //ShowPrediction(prediction);
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void PhotoSelected(UIImage image)
        {
            // clean up previousimage in image view
            if (ImageView.Image != null)
            {
                ImageView.Image.Dispose();
            }

            ImageView.Image = image;

            this.ShowMessage("Took Photo");

        }

        private void ReplaceItem()
        {

        }

        public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue (segue, sender);

            var addItemController = segue.DestinationViewController as AddItemViewController;
            
            if (addItemController != null)
            {
                addItemController.Image = ImageView.Image;
            }
        }
    }
}