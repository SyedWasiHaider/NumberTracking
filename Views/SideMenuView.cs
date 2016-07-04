using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NumberTracker
{
	public class SideMenuView : ContentPage
	{
		public ListView ListView
		{
			get;set;
		}

		public SideMenuView()
		{
			var masterPageItems = new List<MasterPageItem>();
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Home",
				TargetType = typeof(NumberTrackerPage)
			});

			masterPageItems.Add(new MasterPageItem
			{
				Title = "Settings",
				TargetType = typeof(SettingsPage)
			});

			ListView = new ListView
			{
				ItemsSource = masterPageItems,
				ItemTemplate = new DataTemplate(() =>
				{
					var imageCell = new ImageCell();
					imageCell.SetBinding(TextCell.TextProperty, "Title");
					imageCell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
					return imageCell;
				}),
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorVisibility = SeparatorVisibility.None
			};

			Padding = new Thickness(0, 40, 0, 0);
			Title = "Menu";

			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
					Children = {
						ListView
					}
			};
		}
	}
}


