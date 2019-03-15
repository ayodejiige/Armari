using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Threading.Tasks;
using CoreGraphics;

namespace armari
{
    public partial class AddItemViewController : UIViewController
    {
        private CameraController cam;
        private Classifier classifier;
        private string currentClass;

        protected AddItemViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            AddItemView.Hidden = true;

            base.ViewDidLoad();

            // Setup Logger
            this.ShowMessage("View Loaded");

            // Setup Camera
            cam = new CameraController();
            cam.ImagePicked += (s, e) => PhotoSelected(e.Value);
            cam.ImageCanceled += (s, e) => PhotoCanceled();

            // Setup Classifier
            classifier = new Classifier();

            //Start Camera
            var picker = cam.ShowCamera();
            PresentViewController(picker, true, null);
        }

        private List<string> RunPredictons(UIImage image)
        {
            Prediction prediction = classifier.Classify(image);
            List<string> classes = new List<string>();
            var predictions = prediction.predictions;
            var message = $"{prediction.ModelName} thinks:\n";
            //var topFive = predictions.Take(5);
            foreach (var p in predictions)
            {
                var prob = p.Item1;
                var desc = p.Item2;
                classes.Add(desc);
                message += $"{desc} : {prob.ToString("P") }\n";
            }

            this.ShowMessage(message);

            return classes;
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
            CGSize size = new CGSize(224, 224);
            ImageView.Image = image.Scale(size);

            this.ShowMessage("Took Photo");

            var classes = RunPredictons(ImageView.Image);

            //Setup Class Picker
            var pickerModel = new ClassModel(this, classes);
            ClassPicker.Model = pickerModel;

            //UpdateClassView(prediction);
        }

        public void ReplaceItem(string className)
        {
            currentClass = className;
        }

        private void PhotoCanceled()
        {
            //NewItemViewController controller = Storyboard. InstantiateViewController("NewItemViewController") as NewItemViewController;

            //this.NavigationController.DismissViewController(true, null);
            // Display the new view
            //this.NavigationController.PopToViewController(controller, true);
            this.NavigationController.PopToRootViewController(true);
        }


        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "StoreSegue")
            {
                this.ShowMessage("Storing Cloth");
                // Initialize message handler


                var addingItemController = segue.DestinationViewController as AddingItemViewController;
                if (addingItemController != null)
                {
                    addingItemController.addCategory = currentClass;
                    addingItemController.identifier = "store";
                    addingItemController.image = ImageView.Image;
                }

            }
        }
    }

    public class ClassModel : UIPickerViewModel
    {
        private readonly AddItemViewController controller;
        public List<string> classes;

        public ClassModel(AddItemViewController controller_, List<string> classes_)
        {
            this.classes = classes_;
            this.controller = controller_;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return classes.Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return classes[(int)row];
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            Application.logger.Message("Selected new");
            controller.ReplaceItem(classes[(int)pickerView.SelectedRowInComponent(0)]);
        }

        public override nfloat GetComponentWidth(UIPickerView picker, nint component)
        {
            if (component == 0)
                return 240f;
            else
                return 40f;
        }

        public override nfloat GetRowHeight(UIPickerView picker, nint component)
        {
            return 40f;
        }
    }
}