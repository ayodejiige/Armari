﻿using System;

using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using CoreGraphics;
using UIKit;
using Foundation;

namespace armari
{
    public partial class DayCollectionViewController : UIViewController
    {
        public static CalendarOutfit outfit;

        public DayCollectionViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        private void AddView()
        {
            this.ShowMessage("Add new view");
            var pageWidth = DayScrollView.Bounds.Width;
            var pageHeight = DayScrollView.Bounds.Height;

            var view = new UIView(new CGRect(0, 0, pageWidth, pageHeight));
            view.BackgroundColor = ArmariColors.EDA31D;

            UIButton button = new UIButton();
            CGRect ScreenBounds = UIScreen.MainScreen.Bounds;
            float buttonWidth = (float)ScreenBounds.X / 2;
            button.Frame = new CGRect(0f, 0f, buttonWidth, 50f);
            button.SetTitle("Select", UIControlState.Normal);

            view.AddSubview(button);

            DayScrollView.AddSubview(view);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.ShowMessage("View Appeared");

            //var pageWidth = DayScrollView.Bounds.Width;
            //var pageHeight = DayScrollView.Bounds.Height;

            //DayScrollView.ContentSize = new CGSize(4 * pageWidth, pageHeight);
            //DayScrollView.PagingEnabled = true;
            //DayScrollView.ShowsHorizontalScrollIndicator = true;

            //AddView();
            //AddView();
            //AddView();
            //AddView();

            //DayScrollView
            var source = DayCollectionView_.DataSource as DayCollectionSource;
            source.UpdateSource();
            DayCollectionView_.ReloadData();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            // Initialize message handler
            NSIndexPath indexPath = DayCollectionView_.GetIndexPathsForSelectedItems()[0];
            var source = DayCollectionView_.DataSource as DayCollectionSource;
            string identifier = source.Titles[(int)indexPath.Item];
            this.ShowMessage(string.Format("Showing Cloth -> {0}", (int)indexPath.Item));

            var classViewController = segue.DestinationViewController as ClassCollectionViewController;
            if (classViewController != null)
            {
                classViewController.Initialize(identifier);
            }
            else
            {
                this.ShowAlert("Error", "Class failed to load");
            }

        }
    }
}