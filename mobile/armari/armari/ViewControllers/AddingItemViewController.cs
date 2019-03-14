using System;
using System.IO;
using Foundation;
using UIKit;
using System.Threading.Tasks;

namespace armari
{
    public partial class AddingItemViewController : UIViewController
    {
        public string retCategory;
        public int retId;
        public string addCategory;

        public UIImage image;
        private Logger logger;
        public string identifier;
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

            DoneButton.TouchUpInside += DoneButton_TouchUpInside;

            DisplayView.Hidden = true;
            Compartment1.BackgroundColor = ArmariColors.E6D1C8;
            Compartment2.BackgroundColor = ArmariColors.E6D1C8;
            Compartment3.BackgroundColor = ArmariColors.E6D1C8;
            Compartment4.BackgroundColor = ArmariColors.E6D1C8;
        }

        public async override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.ShowMessage("View appeared");

            this.StartLoadingOverlay();

            await Task.Factory.StartNew(() => {
                if (identifier == "store")
                {
                    NewItemInit cloth;
                    cloth.type = addCategory;
                    location = Application.mh.ServiceInit<NewItemInit>(cloth);
                }
                else if (identifier == "ret")
                {
                    if (retCategory == "Dangling")
                    {
                        this.ShowMessage("Returning Item");
                        ReturnItemReq cloth;
                        cloth.id = retId;
                        location = Application.mh.ServiceInit<ReturnItemReq>(cloth);
                    }
                    else
                    {
                        this.ShowMessage("Selecting Item");
                        SelectItemReq cloth;
                        cloth.id = retId;
                        location = Application.mh.ServiceInit<SelectItemReq>(cloth);
                    }
                }
                else if (identifier == "day")
                {
                    this.ShowMessage("Selecting Item");
                    SelectItemReq cloth;
                    cloth.id = retId;
                    location = Application.mh.ServiceInit<SelectItemReq>(cloth);
                }

            });

            this.StopLoadingOverlay();

            if (location.locs == null)
            {
                if (identifier == "store")
                {
                    this.NavigationController.PopToRootViewController(true);
                }
                else
                {
                    this.NavigationController.PopViewController(true);
                }
                this.ShowAlert("Location Error", "Got no location from closet");
            } else if(location.locs.Count == 0)
            {
                this.NavigationController.PopViewController(true);
                this.ShowAlert("Location Error", "Item is not in the wardrobe");
            } else
            {
                DisplayView.Hidden = false;
                Cell cell = location.locs[0];
                if (cell.x == 0 & cell.y == 0)
                {
                    Compartment1.BackgroundColor = ArmariColors.FEC821;
                }
                else if (cell.x == 0 & cell.y == 1)
                {
                    Compartment2.BackgroundColor = ArmariColors.FEC821;
                }
                else if (cell.x == 1 & cell.y == 1)
                {
                    Compartment3.BackgroundColor = ArmariColors.FEC821;
                }
                else if (cell.x == 3 & cell.x == 0)
                {
                    Compartment4.BackgroundColor = ArmariColors.FEC821;
                }
            }

        }

        void DoneButton_TouchUpInside(object sender, EventArgs e)
        {
            Status status;
            status.status = 1;
            Response res = Application.mh.ServiceFinish<Response>(status);
            this.ShowMessage(String.Format("Retrieved ID from pi {0}", res.id));

            if (identifier == "store" & res.status == 1)
            {
                StoreImage(res.id, image);
                this.NavigationController.PopToRootViewController(true);
            }
            else if (identifier == "ret")
            {
                // Initialize message handler
                this.NavigationController.PopToRootViewController(true);
            }
            else if (identifier == "day")
            {
                this.NavigationController.PopViewController(true);
            }

        }


        public void StoreImage(string id, UIImage img)
        {
            string filename = Path.Combine(folderPath, fileName+id+".png");
            NSData image = img.AsPNG();
            NSError err = null;
            
            this.ShowMessage(string.Format("Saving Image to {0}", filename));
            try
            {
                image.Save(filename, false, out err);
            }
            catch (Exception ex)
            {
                this.ShowAlert("File issue", ex.ToString());
            }
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
        }
    }
}

