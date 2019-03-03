using System;
using UIKit;
using Foundation;
using System.Net.Mqtt;
using System.Text;
using System.Threading.Tasks;
using CoreGraphics;

namespace armari
{
    public partial class ViewController : UIViewController
    {
        private CameraController cam;
        private MessageHandler mh;
        private Classifier classifier;
        private Logger logger;
        UIActionSheet actionSheet;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Setup Camera
            //cam = new CameraController(liveCameraStream);
            cam = new CameraController();
            cam.ImagePicked += (s, e) => PhotoButtonTapped(e.Value);

            mh = new MessageHandler();
            classifier = new Classifier();

            // Setup Logger
            logger = Logger.Instance;
            logger.ErrorOccurred += (s, e) => ShowAlert("Processing Error", e.Value);
            logger.MessageUpdated += (s, e) => ShowMessage(e.Value);

            // Setup action sheet
            actionSheet = new UIActionSheet("action sheet with other buttons");
            actionSheet.AddButton("done");
            actionSheet.AddButton("cancel");
            actionSheet.DestructiveButtonIndex = 0;
            actionSheet.CancelButtonIndex = 1;
            actionSheet.Clicked += delegate (object a, UIButtonEventArgs b)
            {
                Console.WriteLine("Button " + b.ButtonIndex.ToString() + " clicked");
                Status status = new Status();
                status.status = 1;

                mh.NewItemFinish("1", status);

                ImageView.Hidden = false;
                ClassificationLabel.Hidden = false;
            };

            // Update UI
            UploadButton.Hidden = true;
            TakePhotoButton.Hidden = true;
            //TakePhotoButton.TouchUpInside += TakePhotoButtonTapped;

            // Initialise camera
            //cam.Init();
        }

        partial void AddButtonTapped(UIButton sender)
        {
            // Update UI
            if (ImageView.Image != null)
            {
                ImageView.Image.Dispose();
            }
            //ImageView.Hidden = true;
            var picker = cam.ShowCamera();

            // Display camera image
            //picker.ShowsCameraControls = true;
            //CGRect rectangle = new CGRect(0, 100, 320, 100);


            //var overlay = new CameraOverlayView();
            //overlay.Draw(rectangle);
            ////overlay.Text = "This is an overlay";
            //picker.CameraOverlayView = new CameraOverlayView(UIScreen.MainScreen.ApplicationFrame);
            //picker.CameraOverlayView.Draw(rectangle);
            PresentViewController(picker, true, null);

            //ClassificationLabel.Hidden = true;
            //AddButton.Hidden = true;
            //TakePhotoButon.Hidden = false;

            //cam.StartStream(this.View.Frame);
            //this.View.DrawRect();
        }

        //To Do: 
        //private async void TakePhotoButtonTapped(object sender, EventArgs e)
        //{
        //    Console.WriteLine("Taking Photo");
        //    var buffer = await cam.TakePhoto();
        //    CGSize size = new CGSize(224, 224);
        //    UIImage image = CameraController.ToUIImage(buffer);
        //    UIImage imageDisplay = image.Scale(size);
        //    //image = CameraController.ConvertToGrayScale(image);
        //    ImageView.Image = imageDisplay;

        //    Console.WriteLine("Taken Photo");

        //    // Run ML model
        //    Prediction prediction = classifier.Classify(image);
        //    ShowPrediction(prediction); 

        //    // Update UI
        //    TakePhotoButton.Hidden = true;
        //    UploadButton.Hidden = false;

        //}

        private void PhotoButtonTapped(UIImage image)
        {
            ShowMessage("Taking Photo");
            CGSize size = new CGSize(224, 224);
            UIImage imageSize = image.Scale(size);
            ImageView.Image = imageSize;
            ShowMessage("Taken Photo");

            // Run ML model
            Prediction prediction = classifier.Classify(image);
            ShowPrediction(prediction);

        }


        partial void UploadButtonTapped(UIButton sender)
        {
            //Cloth cloth = new Cloth();
            //cloth.type = "dress";
            //var task = Task.Run(() => mh.NewItemInit("1", cloth));

            //// Update UI
            //actionSheet.ShowInView(View);
            //AddButton.Hidden = false;
            //UploadButton.Hidden = true;

            //cam.StopStream();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }


        private void ShowPrediction(Prediction prediction)
        {
            var message = $"{prediction.ModelName} thinks:\n";
            //var topFive = prediction.predictions.Take(5);
            foreach (var p in prediction.predictions)
            {
                var prob = p.Item1;
                var desc = p.Item2;
                message += $"{desc} : {prob.ToString("P") }\n";
            }

            ClassificationLabel.Text = message;
        }
        public void ShowAlert(string title, string message)
        {
            //Create Alert
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            //Add Action
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            // Present Alert
            PresentViewController(okAlertController, true, null);
        }

        void ShowMessage(string msg)
        {
            Console.WriteLine("armari -> {0}", msg);
            InvokeOnMainThread(() => Debug.Text = msg);
        }
    }
}
