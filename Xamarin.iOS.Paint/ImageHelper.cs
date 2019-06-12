using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Xamarin.iOS.Paint
{
    public static class ImageHelper
    {
        public static string ImageToBase64(UIImage image)
        {
            if (image == null) return string.Empty;
            var base64String = "";
            var imageData = image.AsPNG();
            base64String = imageData.GetBase64EncodedString(NSDataBase64EncodingOptions.EndLineWithLineFeed);
            return base64String;
        }

        public static UIImage Base64ToImage(string imageString)
        {
            var imageData = new NSData(imageString, NSDataBase64DecodingOptions.IgnoreUnknownCharacters);
            var image = new UIImage(imageData);
            return image;
        }
    }
}