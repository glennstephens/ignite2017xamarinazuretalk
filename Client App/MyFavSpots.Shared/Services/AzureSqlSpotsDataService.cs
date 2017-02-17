using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace MyFavSpots
{
	public class AzureSqlSpotsDataService : ISpotsDataService
	{
		IMobileServiceTable<FavouriteSpot> favouriteSpotsTable;

		MobileServiceClient client;

		public AzureSqlSpotsDataService(MobileServiceClient client)
		{
			this.client = client;
		}

		void Initialize()
		{
			if (favouriteSpotsTable != null)
				return;
			
			favouriteSpotsTable = client.GetTable<FavouriteSpot>();
		}

		public async Task<bool> SaveFavoriteSpotAsync(FavouriteSpot spot)
		{
			Initialize();

			if (string.IsNullOrEmpty(spot.Id))
				await favouriteSpotsTable.InsertAsync(spot);
			else
				await favouriteSpotsTable.UpdateAsync(spot);

			return true;
		}

		public async Task<IList<FavouriteSpot>> SearchForLocationsAsync(string searchText, 
		                                                                double latitude, double longitude)
		{
			Initialize();

			if (String.IsNullOrEmpty(searchText))
				return await favouriteSpotsTable.ToListAsync();
			else
				return await favouriteSpotsTable
					.Where(spot => spot.Name.Contains(searchText)).ToListAsync();
		}
	}
}
