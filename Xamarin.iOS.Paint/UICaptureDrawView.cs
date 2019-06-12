using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.iOS.Paint
{
    public class UICaptureDrawView : UIView
    {
        public delegate void SignedDelegate();

        private readonly CGPoint _baseLineEnd;

        private readonly CGPoint _baseLineStart;
        private readonly NSMutableArray<CustomUIBezierPath> _paths;
        public UIColor LineColor;

        public float LineWidth;

        public UICaptureDrawView(CGRect frame, bool needBaseLine = true) : base(frame)
        {
            _paths = new NSMutableArray<CustomUIBezierPath>();
            NeedBaseLine = needBaseLine;

            if (NeedBaseLine)
            {
                var signatureBaseLine = new CustomUIBezierPath
                {
                    LineWidth = 1,
                    LineColor = UIColor.Black,
                    LineJoinStyle = CGLineJoin.Round
                };
                _baseLineStart = new CGPoint(0, frame.Height - frame.Height / 32);
                _baseLineEnd = new CGPoint(frame.Width, frame.Height - frame.Height / 32);
                signatureBaseLine.MoveTo(_baseLineStart);
                signatureBaseLine.AddLineTo(_baseLineEnd);
                signatureBaseLine.Stroke();
                _paths.Add(signatureBaseLine);
            }
        }


        public bool NeedBaseLine { get; }

        public UIImage SaveAsPicture()
        {
            UIGraphics.BeginImageContextWithOptions(Frame.Size, false, 0);
            Layer.RenderInContext(UIGraphics.GetCurrentContext());
            var signatureImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return signatureImage;
        }

        public void Clean()
        {
            _paths.RemoveAllObjects();

            if (NeedBaseLine)
            {
                var signatureBaseLine = new CustomUIBezierPath
                {
                    LineWidth = 1,
                    LineColor = UIColor.Black,
                    LineJoinStyle = CGLineJoin.Round
                };

                signatureBaseLine.MoveTo(_baseLineStart);
                signatureBaseLine.AddLineTo(_baseLineEnd);
                signatureBaseLine.Stroke();
                _paths.Add(signatureBaseLine);
            }

            SetNeedsDisplay();
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            var touch = (UITouch)touches.AnyObject;
            var point = touch.LocationInView(touch.View);
            var path = new CustomUIBezierPath();
            if (LineWidth <= 0)
                path.LineWidth = 5;
            else
                path.LineWidth = LineWidth;

            path.LineColor = LineColor;
            path.MoveTo(point);
            _paths.Add(path);
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            var touch = (UITouch)touches.AnyObject;
            var point = touch.LocationInView(touch.View);
            _paths.Last().AddLineTo(point);
            SetNeedsDisplay();
        }

        public override void Draw(CGRect rect)
        {
            foreach (var path in _paths)
            {
                path.LineColor.SetColor();
                path.LineJoinStyle = CGLineJoin.Round;
                path.LineCapStyle = CGLineCap.Round;
                path.Stroke();
            }
        }
    }
}