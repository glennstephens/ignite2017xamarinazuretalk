using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFavSpots.Shared;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MyFavSpots
{
	public partial class DisplaySpotsPage : ContentPage
	{
		ISpotsDataService Data;

		public DisplaySpotsPage(ISpotsDataService data)
		{
			Data = data;

			InitializeComponent();

			if (!CrossMedia.Current.IsCameraAvailable)
				ToolbarItems.Remove(takePhotoButton);

			if (!CrossMedia.Current.IsPickPhotoSupported)
				ToolbarItems.Remove(chooseImageButton);

			this.spotList.ItemTemplate = new DataTemplate(typeof(FavSpotCell));
		}

		async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentDevicePositionAsync()
		{
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
			return await locator.GetPositionAsync(5000);
		}

		async Task PerformSearchAsync()
		{
			var dlg = DependencyService.Get<IShowInProgressDialog>();
			dlg.Show("Searching for the good spots...");

			var position = await GetCurrentDevicePositionAsync();

			var items = await Data.SearchForLocationsAsync(searchFor.Text, position.Latitude, position.Longitude);

			dlg.Hide();

			spotList.ItemsSource = items;
		}

		async void SearchForSpots(object sender, System.EventArgs e)
		{
			await PerformSearchAsync();
		}

		async void Handle_Refreshing(object sender, System.EventArgs e)
		{
			await PerformSearchAsync();

			spotList.IsRefreshing = false;
		}

		async void AddNewPhoto(object sender, System.EventArgs e)
		{
			await SelectImage(true);
		}

		async void AddNewLibraryImage(object sender, System.EventArgs e)
		{
			await SelectImage(false);
		}

		async Task SelectImage(bool selectImageFromCamera)
		{
			MediaFile image;

			if (selectImageFromCamera)
			{
				image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
				{
					AllowCropping = true,
					DefaultCamera = CameraDevice.Rear,
					CompressionQuality = 60,
				});
			}
			else {
				image = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
				{
					CompressionQuality = 60
				});
			}

			if (image == null)
				return;

			var dlg = DependencyService.Get<IShowInProgressDialog>();
			dlg.Show("Processing the image...");

			var spot = new FavouriteSpot()
			{
				Name = "Spot",
				RatingOutOfFive = 5,
				AddedById = App.AuthenticatedUser != null ? App.AuthenticatedUser.UserId : "",
			};

			var processingTask = GetCurrentDevicePositionAsync().ContinueWith((locationTask) =>
			{
				var location = locationTask.Result;

				spot.Latitude = location.Latitude;
				spot.Longitude = location.Longitude;

				new Geocoder()
					.GetAddressesForPositionAsync(new Position(location.Latitude, location.Longitude))
					.ContinueWith((possibleAddressMatches) =>
					{
						if (possibleAddressMatches.Result.Count() > 0)
						{
							spot.Address = possibleAddressMatches.Result.First();
						}
					});
			});

			// Get the description of the image using Azure Cognative Services 
			var visionClient = new Microsoft.ProjectOxford.Vision.VisionServiceClient(MobileServiceClientConstants.CognativeServicesVisionApiKey);

			var processImage = visionClient.DescribeAsync(image.GetStream()).ContinueWith((imageDescriptionTask) =>
			{
				// Process the Cognative Services Result
				var suggestedCategories = String.Join(", ", imageDescriptionTask.Result.Description.Tags);
				if (String.IsNullOrEmpty(spot.Category))
					spot.Category = suggestedCategories;

				var suggestedDescription = imageDescriptionTask.Result.Description.Captions.First();
				if (suggestedDescription != null && String.IsNullOrEmpty(spot.Description))
					spot.Description = suggestedDescription.Text;
			});

			// Wait for the operations to finish
			await Task.WhenAll(processingTask, processImage);

			dlg.Hide();

			// Show the edit page to let them save the record or not
			var editPage = new AddEditSpotPage(Data, spot, image);
			await Navigation.PushAsync(editPage);
		}

		async void ShowSelectedItem(object sender, ItemTappedEventArgs e)
		{
			var selectedSpot = e.Item as FavouriteSpot;

			var page = new DisplayLocationDetailsPage(selectedSpot);
			await Navigation.PushAsync(page);
		}
	}
}
