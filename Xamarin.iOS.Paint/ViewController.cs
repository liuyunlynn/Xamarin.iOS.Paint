using Foundation;
using System;
using UIKit;

namespace Xamarin.iOS.Paint
{
    public partial class ViewController : UIViewController
    {
        public ViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            img.ContentMode = UIViewContentMode.ScaleAspectFit;
            var imageTapGestureRecognizer = new UITapGestureRecognizer(() =>
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                if (!(storyboard.InstantiateViewController("DrawingBoardViewController") is
                    DrawingBoardViewController drawingBoardViewController)) return;


                drawingBoardViewController.SaveImage = (imageString) =>
                {
                    img.Image = ImageHelper.Base64ToImage(imageString);
                 
                };
                NavigationController.PushViewController(drawingBoardViewController, true);
            });

            img.UserInteractionEnabled = true;
            img.AddGestureRecognizer(imageTapGestureRecognizer);
        }

        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}