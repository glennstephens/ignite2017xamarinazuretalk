using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json;
using Plugin.Connectivity;

namespace MyFavSpots.Shared
{
	public class AzureOfflineSpotsDataService : ISpotsDataService
	{
		IMobileServiceSyncTable<FavouriteSpot> favouriteSpotsTable;

		MobileServiceClient client;

		public AzureOfflineSpotsDataService(MobileServiceClient client)
		{
			this.client = client;
		}

		async Task Initialize()
		{
			if (favouriteSpotsTable != null)
				return;
			
			var store = new MobileServiceSQLiteStore("localstore.db");
			store.DefineTable<FavouriteSpot>();
			await client.SyncContext.InitializeAsync(store,
										new MobileServiceSyncHandler());


			favouriteSpotsTable = client.GetSyncTable<FavouriteSpot>();
		}

		public async Task<bool> SaveFavoriteSpotAsync(FavouriteSpot spot)
		{
			await Initialize();

			if (string.IsNullOrEmpty(spot.Id))
				await favouriteSpotsTable.InsertAsync(spot);
			else
				await favouriteSpotsTable.UpdateAsync(spot);

			await SynchronizeAsync();

			return true;
		}

		private async Task SynchronizeAsync()
		{
			if (!CrossConnectivity.Current.IsConnected)
				return;

			try
			{
				await client.SyncContext.PushAsync();
				await favouriteSpotsTable.PullAsync(null, favouriteSpotsTable.CreateQuery());
			}
			catch (Exception ex)
			{
				// TODO: handle error in your app specific way
				Console.WriteLine("Could not sync: " + ex.Message);
			}
		}

		public async Task<IList<FavouriteSpot>> SearchForLocationsAsync(string searchText, double latitude, double longitude)
		{
			await Initialize();

			List<FavouriteSpot> items;

			if (String.IsNullOrEmpty(searchText))
				items = await favouriteSpotsTable.ToListAsync();
			else
				items = await favouriteSpotsTable
					.Where(spot => spot.Name.Contains(searchText)).ToListAsync();

			return items
				.Select(spot => new FavouriteSpot()
				{
					Id = spot.Id,
					Name = spot.Name,
					Address = spot.Address,
					Category = spot.Category,
					Description = spot.Description,
					Latitude = spot.Latitude,
					Longitude = spot.Longitude,
					AddedById = spot.AddedById,
					MainImageUrl = spot.MainImageUrl,
					RatingOutOfFive = spot.RatingOutOfFive
				})
				.ToList();
		}
	}
}
