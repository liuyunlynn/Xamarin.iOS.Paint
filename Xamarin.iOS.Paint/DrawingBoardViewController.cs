using Foundation;
using System;
using CoreGraphics;
using UIKit;

namespace Xamarin.iOS.Paint
{
    public partial class DrawingBoardViewController : UIViewController
    {
        public delegate void SaveDrawImageDelegate(string imageString);
        public SaveDrawImageDelegate SaveImage { get; set; }

        private readonly bool needDrawBaseline = true;
        public DrawingBoardViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                ((AppDelegate)UIApplication.SharedApplication.Delegate).AllowRotation = true;
                ((AppDelegate)UIApplication.SharedApplication.Delegate).SwitchNewOrientation(UIInterfaceOrientation
                    .LandscapeLeft);



                var captureDrawView =
                    new UICaptureDrawView(new CGRect(5, 5, View.Frame.Height - 10,
                        View.Frame.Width / 4 * 3), needDrawBaseline)
                    {
                        BackgroundColor = UIColor.Clear,
                        LineWidth = 3,
                        LineColor = UIColor.Black,
                    };
                View.AddSubview(captureDrawView);
                if (!needDrawBaseline)
                {
                    var baseline = new UIView(new CGRect(5,
                        captureDrawView.Frame.Y + captureDrawView.Frame.Height, View.Frame.Height - 10, 1))
                    {
                        BackgroundColor = UIColor.Black
                    };
                    View.AddSubview(baseline);
                }

                var btnClear = new UIBarButtonItem { Title = "Clear" };
                NavigationItem.RightBarButtonItem = btnClear;
                btnClear.Clicked += (sender, e) => { captureDrawView.Clean(); };


                var buttonWidth = (View.Frame.Height - 3 * 20f) / 2;
                var btnSave = new UIButton(new CGRect(View.Frame.Height / 2 + 10,
                    captureDrawView.Frame.Y + captureDrawView.Frame.Height + 10, buttonWidth, 40f));
                btnSave.SetTitle("Save", UIControlState.Normal);
                btnSave.Layer.CornerRadius = 7;
                btnSave.SetTitleColor(UIColor.Blue, UIControlState.Normal);
                btnSave.BackgroundColor = UIColor.Green;
                btnSave.TouchUpInside += (sender, e) =>
                {


                    var DrawImage = captureDrawView.SaveAsPicture();
                    SaveImage(ImageHelper.ImageToBase64(DrawImage));
                    NavigationController.PopViewController(true);
                };
                View.AddSubview(btnSave);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);
                NavigationController.InteractivePopGestureRecognizer.Enabled = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            try
            {
                base.ViewWillDisappear(animated);
                NavigationController.InteractivePopGestureRecognizer.Enabled = true;

                ((AppDelegate)UIApplication.SharedApplication.Delegate).AllowRotation = false;
                ((AppDelegate)UIApplication.SharedApplication.Delegate).SwitchNewOrientation(UIInterfaceOrientation
                    .Portrait);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}