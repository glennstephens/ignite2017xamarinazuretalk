using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFavSpots
{
	public interface ISpotsDataService
	{
		Task<IList<FavouriteSpot>> SearchForLocationsAsync(string searchText, double latitude, double longitude);
		Task<bool> SaveFavoriteSpotAsync(FavouriteSpot spot);
	}
}
