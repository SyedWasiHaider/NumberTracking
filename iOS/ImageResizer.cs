using System;
using System.Drawing;
using CoreGraphics;
using UIKit;

namespace NumberTracker.iOS
{
	public class ImageResizer : IImageResizer
	{
		public ImageResizer()
		{
		}

		byte[] IImageResizer.Resize(byte[] imageData, int width, int height)
		{
			UIImage originalImage = ImageFromByteArray(imageData);
			UIImageOrientation orientation = originalImage.Orientation;

			//create a 24bit RGB image
			using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
												 (int)width, (int)height, 8,
												 (int)(4 * width), CGColorSpace.CreateDeviceRGB(),
												 CGImageAlphaInfo.PremultipliedFirst))
			{

				RectangleF imageRect = new RectangleF(0, 0, width, height);

				// draw the image
				context.DrawImage(imageRect, originalImage.CGImage);

				UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

				// save the image as a jpeg
				return resizedImage.AsJPEG().ToArray();
			}
		}

		public UIKit.UIImage ImageFromByteArray(byte[] data){
			if (data == null)
			{
				return null;
			}

			UIKit.UIImage image;
			try
			{
				image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
			}
			catch (Exception e)
			{
				Console.WriteLine("Image load failed: " + e.Message);
				return null;
			}
			return image;
		}

	}
}

