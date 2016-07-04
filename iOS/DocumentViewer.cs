using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Foundation;
using NumberTracker.iOS;
using QuickLook;
using UIKit;

namespace NumberTracker.iOS
{
	public class DocumentViewer : IDocumentViewer
	{
		public void ShowDocumentFile(string filepath, string mimeType)
		{
			var fileinfo = new FileInfo(filepath);
			var previewController = new QLPreviewController();
			previewController.DataSource = new PreviewControllerDataSource(fileinfo.FullName, fileinfo.Name);

			var controller = FindNavigationController();

			if (controller != null)
			{
				controller.PresentViewController((UIViewController)previewController, true, (Action)null);
			}
		}

		private UIViewController FindNavigationController()
		{
			foreach (var window in UIApplication.SharedApplication.Windows)
			{
				if (window.RootViewController.NavigationController != null)
				{
					return window.RootViewController.NavigationController;
				}
				else
				{
					UINavigationController value = CheckSubs(window.RootViewController.ChildViewControllers);
					if (value != null)
					{
						return value;
					}
				}
			}
			return UIApplication.SharedApplication.Windows.First().RootViewController;
		}

		private UINavigationController CheckSubs(UIViewController[] controllers)
		{
			foreach (var controller in controllers)
			{
				if (controller.NavigationController != null)
				{
					return controller.NavigationController;
				}
				else
				{
					UINavigationController value = CheckSubs(controller.ChildViewControllers);

					if (value != null)
					{
						return value;
					}
				}

				return null;
			}
			return null;
		}
	}

	public class DocumentItem : QLPreviewItem
	{
		private string _title;
		private string _uri;

		public DocumentItem(string title, string uri)
		{
			_title = title;
			_uri = uri;
		}

		public override string ItemTitle
		{ get { return _title; } }

		public override NSUrl ItemUrl
		{ get { return NSUrl.FromFilename(_uri); } }
	}

	public class PreviewControllerDataSource : QLPreviewControllerDataSource
	{
		private string _url;
		private string _filename;

		public PreviewControllerDataSource(string url, string filename)
		{
			_url = url;
			_filename = filename;
		}

		public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
		{
			return (IQLPreviewItem)new DocumentItem(_filename, _url);
		}

		public override nint PreviewItemCount(QLPreviewController controller)
		{ return (nint)1; }
	}
}

