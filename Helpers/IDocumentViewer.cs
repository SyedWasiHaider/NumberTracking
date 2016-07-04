using System;
namespace NumberTracker
{
	//Interface in PCL
	public interface IDocumentViewer
	{
		void ShowDocumentFile(string filepaht, string mimeType);
	}
}

