using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFavSpots
{
	public class MockDataService : ISpotsDataService
	{
		public async Task<bool> SaveFavoriteSpotAsync(FavouriteSpot spot)
		{
			FixedSpots.Add(spot);

			return await Task.FromResult(true);
		}

		public async Task<IList<FavouriteSpot>> SearchForLocationsAsync(string searchText, double latitude, double longitude)
		{
			if (String.IsNullOrEmpty(searchText))
				return new List<FavouriteSpot>();

			return await Task.FromResult(
				FixedSpots
					.Where(spot => spot.Name.ToLower().Contains(searchText.ToLower()))
			                             .ToList());
		}

		static readonly List<FavouriteSpot> FixedSpots = new List<FavouriteSpot>(new[] {
			new FavouriteSpot() {
				Id = "1",
				Address = "Esplanade, Surfers Paradise",
				Category = "Outdoors",
				Description = "A great beach for just relaxing",
				Latitude = -27.997938,
				Longitude = 153.430690,
				Name = "The Esplanade",
				RatingOutOfFive = 4,
				MainImageUrl = "http://www.oneontheesplanade.com.au/wp/wp-content/uploads/2013/12/surfers-paradise-accommodation3-951x390.jpg",
				AddedById = "1"
			},
			new FavouriteSpot() {
				Id = "2",
				Address = "Waterways Drive, Main Beach",
				Category = "Outdoors",
				Description = "Do some sweet flying",
				Latitude = -27.977381,
				Longitude = 153.423010,
				Name = "Jetpack Adventures",
				RatingOutOfFive = 5,
				MainImageUrl = "https://media-cdn.tripadvisor.com/media/photo-s/07/99/6f/91/gold-coast-watersports.jpg",
				AddedById = "1"
			},
			new FavouriteSpot() {
				Id = "3",
				Address = "Lamington National Park",
				Category = "Nature",
				Description = "Some great waterfalls. You have to take your own lamingtons",
				Latitude = -28.008612,
				Longitude = 153.257454,
				Name = "Coomera Falls",
				RatingOutOfFive = 4,
				MainImageUrl = "http://www.visitgoldcoast.com/Portals/0/Images/Blog/GarethMcGuigan/Coomera_Falls.jpg?ver=2015-10-12-081146-467",
				AddedById = "1"
			}
		});
	}
}
