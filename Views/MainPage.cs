using System;

using Xamarin.Forms;

namespace NumberTracker
{
	public class MainPage : MasterDetailPage
	{
		SideMenuView sideMenu;

		public MainPage()
		{
			sideMenu = new SideMenuView();
			Master = sideMenu;
			Detail = new NavigationPage(new NumberTrackerPage()) {BarTextColor=Color.White, BarBackgroundColor = ColorStyles.AccentColor };

			sideMenu.ListView.ItemSelected += OnItemSelected;
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MasterPageItem;
			if (item != null)
			{
				Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType)){ BarTextColor = Color.White, BarBackgroundColor = ColorStyles.AccentColor };
				sideMenu.ListView.SelectedItem = null;
				IsPresented = false;
			}
		}
	}
}


