using System;
using System.IO;
using Android.Graphics;

namespace NumberTracker.Droid
{
	public class ImageResizer : IImageResizer
	{
		public ImageResizer()
		{
		}

		public byte[] Resize(byte[] imageData, int width, int height)
		{
			// Load the bitmap
			Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, width, height, false);

			using (MemoryStream ms = new MemoryStream())
			{
				resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);
				return ms.ToArray();
			}
		}
	}
}

