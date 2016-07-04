using System;
using System.Collections.Generic;
using System.Linq;
using NumberTracker.Helpers;
using Realms;
using Xamarin.Forms;

namespace NumberTracker
{
	public partial class SettingsPage : ContentPage
	{

		public SettingsPage()
		{
			InitializeComponent();


			switchAction.IsToggled = Settings.EnableDefaultAction;
			picker.IsEnabled = Settings.EnableDefaultAction;
			switchAction.Toggled += (sender, e) =>
			{
				Settings.EnableDefaultAction = switchAction.IsToggled;
				picker.IsEnabled = Settings.EnableDefaultAction;
			};

			picker.Items.Add(Settings.CAMERA);
			picker.Items.Add(Settings.GALLERY);

			if (Settings.DefaultCameraSelection == Settings.CAMERA)
			{
				picker.SelectedIndex = 0;
			}
			else {
				picker.SelectedIndex = 1;
			}

			picker.SelectedIndexChanged += (sender, e) =>
			{
				if (picker.SelectedIndex == 0)
				{
					Settings.DefaultCameraSelection = Settings.CAMERA;
				}
				else {
					Settings.DefaultCameraSelection = Settings.GALLERY;
				}
			};

			nukeButton.Clicked += async (sender, e) =>
			{
				var ok = await DisplayAlert("Warning", "This will delete all the data you've collected.", "Yes", "No");
				if (ok)
				{
					using (var realm = Realm.GetInstance())
					{
						while (realm.All<Transaction>().Count() > 0)
						{
							var transaction = realm.All<Transaction>().First();

							// Delete an object with a transaction
							using (var trans = realm.BeginWrite())
							{
								realm.Remove(transaction);
								trans.Commit();
							}
						}
					}

				}
			};


		}
	}
}

