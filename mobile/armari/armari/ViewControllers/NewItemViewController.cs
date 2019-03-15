using System;

using UIKit;

namespace armari
{
    public partial class NewItemViewController : UIViewController
    {
        protected NewItemViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Setup Logger
            this.ShowMessage("View Loaded");
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

