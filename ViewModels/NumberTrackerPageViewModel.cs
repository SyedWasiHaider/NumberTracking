using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PCLStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Share;
using Realms;
using Xamarin.Forms;

namespace NumberTracker
{
	public class NumberTrackerPageViewModel : BaseViewModel
	{
		IDocumentViewer docViewer;
		public const string SHOW_DIALOG_MESSAGE = "showDialog";

		public NumberTrackerPageViewModel()
		{
			docViewer = DependencyService.Get<IDocumentViewer>();

			TakePhotoCommand = new Command(async () =>
			{
				var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() { Name = "myimage" });
				await setNumberText(photo);
			});
			PickPhotoCommand = new Command(async () =>
			{
				var photo = await CrossMedia.Current.PickPhotoAsync();
				await setNumberText(photo);
			});
			VerifyAddCommand = new Command(() =>
			{
				AddTransaction();
				NumberText = String.Empty;
				ImagePath = String.Empty;
			});

			DeleteCommand = new Command((record) =>
			{
				var transaction = (TransactionViewModel)record;
				var id = transaction.id;
				using (var realm = Realm.GetInstance())
				{
					var myTransaction = realm.All<Transaction>().Where(elem => elem.transactionId == id).First();
					// Delete an object with a transaction
					using (var trans = realm.BeginWrite())
					{
						realm.Remove(myTransaction);
						trans.Commit();
					}
				}
				SetItems();
			});

			SaveCsvCommand = new Command(async ()=> await SaveAsCsvAsync());
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
				docViewer.ShowDocumentFile(file.Path, "text/csv");
			}
		}

		public void SetItems()
		{
			Transactions.Clear();
			using (var realm = Realm.GetInstance())
			{
				var temp = realm.All<Transaction>().OrderByDescending(c => c.DateTimeAdded);
				foreach (var element in temp)
				{
					Transactions.Add(new TransactionViewModel(element));
				}
			}
		}


		public void AddTransaction()
		{
			if (String.IsNullOrEmpty(NumberText))
			{
				MessagingCenter.Send<NumberTrackerPageViewModel, string>(this, SHOW_DIALOG_MESSAGE, "No Text");
				return;
			}

			using (var realm = Realm.GetInstance())
			{

				 string lol = NumberText;
				if (realm.All<Transaction>().Where(record => record.transactionId == lol).Count() > 0)
				{
					MessagingCenter.Send<NumberTrackerPageViewModel, string>(this, SHOW_DIALOG_MESSAGE, $"Transaction number {NumberText} already exists");
				}
				else {
					realm.Write(() =>
					{
						var transaction = realm.CreateObject<Transaction>();
						transaction.DateTimeAdded = DateTimeOffset.UtcNow;
						transaction.transactionId = NumberText;
					});
				}
			}

			SetItems();
		}

		public async Task setNumberText(MediaFile mediaFile)
		{
			if (mediaFile == null)
			{
				return;
			}
			NumberText = String.Empty;
			ImagePath= mediaFile.Path;
			var text = await ImageProcessingHelper.GetTextFromImage(mediaFile);
			var regex = new Regex(@"\d{10,12}");
			var match = regex.Match(text);
			var numberStr = match.ToString();
			NumberText = numberStr;
		}

		public Command TakePhotoCommand { get; set; }
		public Command PickPhotoCommand { get; set; }
		public Command VerifyAddCommand { get; set; }
		public Command SaveCsvCommand { get; set; }
		public Command DeleteCommand { get; set; }

		private string _imagePath = String.Empty;
		public string ImagePath
		{
			get
			{
				if (String.IsNullOrEmpty(_imagePath))
				{
					return "camera.png";
				}
				return _imagePath;
			}
			set
			{
				_imagePath = value;
				RaisePropertyChanged();
			}

		}

		private string _numberText = String.Empty;
		public string NumberText
		{
			get
			{
				return _numberText;
			}
			set
			{
				_numberText = value;
				RaisePropertyChanged();
			}

		}

		ObservableCollection<TransactionViewModel> _transaction = new ObservableCollection<TransactionViewModel>();
		public ObservableCollection<TransactionViewModel> Transactions
		{
			get
			{
				return _transaction;
			}

			set
			{
				_transaction = value;
				RaisePropertyChanged();
			}
		}

	}
}

