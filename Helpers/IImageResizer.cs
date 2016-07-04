using System;
namespace NumberTracker
{
	public interface IImageResizer
	{
		 byte[] Resize(byte [] imageData, int width, int height);
	}
}

