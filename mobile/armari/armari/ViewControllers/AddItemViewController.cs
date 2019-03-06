using System;
using Foundation;
using UIKit;

namespace armari
{
    public partial class AddItemViewController : UIViewController
    {
        private CameraController cam;
        private MessageHandler mh;
        private Classifier classifier;
        private Logger logger;

        protected AddItemViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            AddItemView.Hidden = true;

            base.ViewDidLoad();

            // Setup Logger
            logger = Logger.Instance;
            logger.ErrorOccurred += (s, e) => this.ShowAlert("Processing Error", e.Value);
            logger.MessageUpdated += (s, e) => this.ShowMessage(e.Value);
            logger.Message("View Loaded");

            // Setup Camera
            cam = new CameraController();
            cam.ImagePicked += (s, e) => PhotoSelected(e.Value);
            cam.ImageCanceled += (s, e) => PhotoCanceled();

            // Setup Classifier
            classifier = new Classifier();
            // UI Setup
            //CameraView.BackgroundColor = ArmariColors.F1F1F2;

            //Button Callbacks

            //Start Camera
            var picker = cam.ShowCamera();
            PresentViewController(picker, true, null);
        }

        private String RunPredictons(UIImage image)
        {
            Prediction prediction = classifier.Classify(image);
            var predictions = prediction.predictions;
            var message = $"{prediction.ModelName} thinks:\n";
            //var topFive = predictions.Take(5);
            foreach (var p in predictions)
            {
                var prob = p.Item1;
                var desc = p.Item2;
                message += $"{desc} : {prob.ToString("P") }\n";
            }

            logger.Message(message);

            //PredictionText.Text = message;

            return predictions[0].Item2;
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void PhotoSelected(UIImage image)
        {
            AddItemView.Hidden = false;

            // clean up previousimage in image view
            if (ImageView.Image != null)
            {
                ImageView.Image.Dispose();
            }
            ImageView.Image = image;

            this.ShowMessage("Took Photo");

            var prediction = RunPredictons(ImageView.Image);
            UpdateClassView(prediction);
        }

        public void UpdateClassView(string className)
        {
            if (ClothClassImage.Image != null)
            {
                ClothClassImage.Image.Dispose();
            }
            ClothClassImage.Image = ClassIcons.Icons["coat"];
            ClothClassLabel.Text = className;
        }

        private void PhotoCanceled()
        {
            NewItemViewController controller = Storyboard.InstantiateViewController("NewItemViewController") as NewItemViewController;

            // Display the new view
            this.NavigationController.PushViewController(controller, true);
        }

        private void ReplaceItem()
        {

        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            //var addItemController = segue.DestinationViewController as AddItemViewController;

            //if (addItemController != null)
            //{
            //    addItemController.Image = ImageView.Image;
            //    addItemController.Prediction = RunPredictons(ImageView.Image);
            //}
        }
    }
}