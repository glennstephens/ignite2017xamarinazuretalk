using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media.Abstractions;
using MyFavSpots.Shared;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MyFavSpots
{
	public partial class AddEditSpotPage : ContentPage
	{
		readonly ISpotsDataService data;
		readonly FavouriteSpot spot;
		readonly MediaFile image;

		public AddEditSpotPage(ISpotsDataService data, FavouriteSpot spot, MediaFile image)
		{
			this.image = image;
			this.spot = spot;
			this.data = data;

			BindingContext = spot;

			InitializeComponent();

			mainLayout.Children.Insert(0, new FavSpotView());

			SaveImageToAzureBlobStorageAsync();
		}

		async void SaveSpot(object sender, System.EventArgs e)
		{
			var dlg = DependencyService.Get<IShowInProgressDialog>();
			dlg.Show("Saving your awesome new spot...");

			await data.SaveFavoriteSpotAsync(spot);

			dlg.Hide();

			await Navigation.PopAsync();
		}

		async Task SaveImageToAzureBlobStorageAsync()
		{
			CloudStorageAccount storageAccount = 
				CloudStorageAccount.Parse(MobileServiceClientConstants.CloudStorageConnectionString);

			var blobClient = storageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(MobileServiceClientConstants.CloudStorageContainerName);
			await container.CreateIfNotExistsAsync();

			var newFilename = GenerateRandomImageFilename();
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(newFilename);
			await blockBlob.UploadFromStreamAsync(image.GetStream());

			// Update the image location
			spot.MainImageUrl = String.Format(MobileServiceClientConstants.CloudStorageUrlPath, newFilename);
		}

		string GenerateRandomImageFilename()
		{
			var result = "";
			foreach (var c in Guid.NewGuid().ToString())
				if (Char.IsLetterOrDigit(c))
					result += c;
			return result + ".jpg";
		}
	}
}
