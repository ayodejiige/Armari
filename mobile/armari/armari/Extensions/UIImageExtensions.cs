using System;
using CoreGraphics;
using CoreVideo;
using UIKit;

namespace armari
{
	public static class UIImageExtensions
	{
		public static CVPixelBuffer ToCVPixelBuffer(this UIImage self)
		{
			var attrs = new CVPixelBufferAttributes();
			attrs.CGImageCompatibility = true;
			attrs.CGBitmapContextCompatibility = true;

			var cgImg = self.CGImage;

			var pb = new CVPixelBuffer(cgImg.Width, cgImg.Height, CVPixelFormatType.CV32ARGB, attrs);
			pb.Lock(CVPixelBufferLock.None);
			var pData = pb.BaseAddress;
			var colorSpace = CGColorSpace.CreateDeviceRGB();
			var ctxt = new CGBitmapContext(pData, cgImg.Width, cgImg.Height, 8, pb.BytesPerRow, colorSpace, CGImageAlphaInfo.NoneSkipFirst);
			ctxt.TranslateCTM(0, cgImg.Height);
			ctxt.ScaleCTM(1.0f, -1.0f);
			UIGraphics.PushContext(ctxt);
			self.Draw(new CGRect(0, 0, cgImg.Width, cgImg.Height));
			UIGraphics.PopContext();
			pb.Unlock(CVPixelBufferLock.None);

			return pb;

		}

        public static CVPixelBuffer ToCVPixelBufferGray(this UIImage self)
        {
            var attrs = new CVPixelBufferAttributes();
            attrs.CGImageCompatibility = true;
            attrs.CGBitmapContextCompatibility = true;

            var cgImg = self.CGImage;

            var pb = new CVPixelBuffer(cgImg.Width, cgImg.Height, CVPixelFormatType.CV1Monochrome, attrs);
            pb.Lock(CVPixelBufferLock.None);
            var pData = pb.BaseAddress;
            var colorSpace = CGColorSpace.CreateGenericGray();
            var ctxt = new CGBitmapContext(pData, cgImg.Width, cgImg.Height, 8, pb.BytesPerRow, colorSpace, CGImageAlphaInfo.NoneSkipFirst);
            ctxt.TranslateCTM(0, cgImg.Height);
            ctxt.ScaleCTM(1.0f, -1.0f);
            UIGraphics.PushContext(ctxt);
            self.Draw(new CGRect(0, 0, cgImg.Width, cgImg.Height));
            UIGraphics.PopContext();
            pb.Unlock(CVPixelBufferLock.None);

            return pb;

        }

        public static UIImage CenterCrop(this UIImage self)
        {
            // Use smallest side length as crop square length
            double squareLength = Math.Min(self.Size.Width, self.Size.Height);

            nfloat x, y;
            x = (nfloat)((self.Size.Width - squareLength) / 2.0);
            y = (nfloat)((self.Size.Height - squareLength) / 2.0);

            //This Rect defines the coordinates to be used for the crop
            CGRect croppedRect = CGRect.FromLTRB(x, y, x + (nfloat)squareLength, y + (nfloat)squareLength);

            // Center-Crop the image
            UIGraphics.BeginImageContextWithOptions(croppedRect.Size, false, self.CurrentScale);
            self.Draw(new CGPoint(-croppedRect.X, -croppedRect.Y));
            UIImage croppedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return croppedImage;
        }
    }
}
