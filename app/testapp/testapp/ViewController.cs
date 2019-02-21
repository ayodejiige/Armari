using System;
using UIKit;
using Foundation;
using System.Net.Mqtt;
using System.Text;
using System.Threading.Tasks;

namespace testapp
{
    public partial class ViewController : UIViewController
    {
        private CameraController cam;
        private MessageHandler mh;
        private Classifier classifier;
        UIActionSheet actionSheet;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            cam = new CameraController(liveCameraStream);
            mh = new MessageHandler();
            classifier = new Classifier();

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
            };

            // Update UI
            UploadButton.Hidden = true;
            TakePhotoButton.Hidden = true;
            TakePhotoButton.TouchUpInside += TakePhotoButtonTapped;

            // Initialise camera
            cam.Init();
        }

        partial void AddButtonTapped(UIButton sender)
        {
            cam.StartStream(this.View.Frame);

            // Update UI
            ImageView.Hidden = true;
            AddButton.Hidden = true;
            TakePhotoButton.Hidden = false;
        }

        //To Do: 
        private async void TakePhotoButtonTapped(object sender, EventArgs e)
        {
            Console.WriteLine("Taking Photo");
            var buffer = await cam.TakePhoto();
            //cam.StopStream();
            UIImage image = cam.ToUIImage(buffer);
            ImageView.Image = image;

            Console.WriteLine("Taken Photo");
            classifier.Classify(image);

            // Update UI
            TakePhotoButton.Hidden = true;
            UploadButton.Hidden = false;

        }

        partial void UploadButtonTapped(UIButton sender)
        {
            Cloth cloth = new Cloth();
            cloth.type = "dress";
            var task = Task.Run(() => mh.NewItemInit("1", cloth));

            // Update UI
            actionSheet.ShowInView(View);
            AddButton.Hidden = false;
            UploadButton.Hidden = true;
            ImageView.Hidden = false;

            cam.StopStream();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }
}
