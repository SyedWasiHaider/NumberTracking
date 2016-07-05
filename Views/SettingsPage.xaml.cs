using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTracker.Helpers;
using PCLStorage;
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

			saveAsCsv.Clicked += async (sender, e) =>
			{
				await SaveAsCsvAsync();
			};
		}

		public async Task SaveAsCsvAsync()
		{
			using (var realm = Realm.GetInstance())
			{
				var temp = realm.All<Transaction>().OrderByDescending(c => c.DateTimeAdded).ToList();
				var csv = new StringBuilder();
				foreach (var element in temp)
				{
					var stringFmt = "{0},{1}\n";
					csv.Append(String.Format(stringFmt, element.transactionId, element.DateTimeAdded));
				}
				var fileName = $"transactions{DateTimeOffset.UtcNow.ToFileTime()}.csv";
				var file = await FileSystem.Current.LocalStorage.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
				await file.WriteAllTextAsync(csv.ToString());
				DependencyService.Get<IDocumentViewer>().ShowDocumentFile(file.Path, "text/csv");
			}
		}
	}
}

