using System;
using System.IO;
using Foundation;
using UIKit;

namespace armari
{
    public partial class AddingItemViewController : UIViewController
    {
        public UIImage image;
        private Logger logger;
        public Location location;
        public MessageHandler mh;
        private static string fileName = "amari_";
        private static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        protected AddingItemViewController(IntPtr handle) : base(handle)
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

        }

        public void StoreImage(string id, UIImage img)
        {
            string filename = Path.Combine(folderPath, fileName+id);
            NSData image = img.AsPNG();
            NSError err = null;
            
            this.ShowMessage(string.Format("Saving Image to {0}", filename));
            //try
            //{
            //    image.Save(filename, false, out err);
            //}
            //catch (Exception ex)
            //{
            //    this.ShowAlert("File issue", ex.ToString());
            //}

        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "StoredSegue")
            {
                // Initialize message handler
                Status status;
                status.status = 1;
                Response res = Application.mh.ServiceFinish<Response>(status);
                this.ShowMessage(String.Format("ID from pi {0}", res.id));
                StoreImage(res.id, image);
            } else if (segue.Identifier == "RetrievedSegue")
            {
                // Initialize message handler
                Status status;
                status.status = 1;
                Response res = Application.mh.ServiceFinish<Response>(status);
                this.ShowMessage(String.Format("Retrieved ID from pi {0}", res.id));
            }

        }
    }
}

