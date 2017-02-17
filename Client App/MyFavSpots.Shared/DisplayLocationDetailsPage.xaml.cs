using System;
using System.Collections.Generic;
using Plugin.ExternalMaps;
using Xamarin.Forms;

namespace MyFavSpots.Shared
{
	public partial class DisplayLocationDetailsPage : ContentPage
	{
		readonly FavouriteSpot spot;

		public DisplayLocationDetailsPage(FavouriteSpot spot)
		{
			this.spot = spot;
			BindingContext = spot;

			InitializeComponent();

			mainLayout.Children.Insert(0, new FavSpotView());
		}

		async void NavigateToPlace(object sender, EventArgs e)
		{
			await CrossExternalMaps.Current.NavigateTo(spot.Name, spot.Latitude, spot.Longitude);
		}
	}
}
