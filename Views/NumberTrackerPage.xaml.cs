using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NumberTracker.Helpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Realms;
using Xamarin.Forms;

namespace NumberTracker
{
	public partial class NumberTrackerPage : ContentPage
	{

		NumberTrackerPageViewModel viewModel;
		public NumberTrackerPage()
		{
			InitializeComponent();
			viewModel = new NumberTrackerPageViewModel();
			viewModel.SetItems();
			BindingContext = viewModel;
			MessagingCenter.Subscribe<NumberTrackerPageViewModel, string>(this, NumberTrackerPageViewModel.SHOW_DIALOG_MESSAGE, async (sender, msg) =>
			{
				await DisplayAlert("Error", msg, "Ok");
			});

			var tapGesture = new TapGestureRecognizer();
			tapGesture.Tapped += async (sender, e) =>
			{
				var takePhoto = "Camera";
				var pickPhoto = "Photo Gallery";

				if (Settings.EnableDefaultAction)
				{
					if (Settings.DefaultCameraSelection == Settings.CAMERA)
					{
						viewModel.TakePhotoCommand.Execute(null);
					}
					else {
						viewModel.PickPhotoCommand.Execute(null);
					}
					return;
				}

				var optionSelected = await DisplayActionSheet("Choose Method", "Cancel", null, takePhoto, pickPhoto);
				if (optionSelected == takePhoto)
				{
					viewModel.TakePhotoCommand.Execute(null);
				}
				else if (optionSelected == pickPhoto)
				{
					viewModel.PickPhotoCommand.Execute(null);
				}
			};

			imageNumber.GestureRecognizers.Add(tapGesture);
		}

		public void OnDelete(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			viewModel.DeleteCommand.Execute(mi.CommandParameter);
		}

	}
}

