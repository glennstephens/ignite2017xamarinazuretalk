using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace MyFavSpots
{
	[JsonObject(Title = "FavoriteSpots")]
	public class FavouriteSpot : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = null;

		public string Id
		{
			get;
			set;
		}

		void RaisePropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		string _Name;

		public string Name
		{
			get { return _Name; }
			set
			{
				if (_Name != value)
				{
					_Name = value;
					RaisePropertyChanged();
				}
			}
		}

		string _Description;

		public string Description
		{
			get { return _Description; }
			set
			{
				if (_Description != value)
				{
					_Description = value;
					RaisePropertyChanged();
				}
			}
		}

		string _AddedById;

		public string AddedById
		{
			get { return _AddedById; }
			set
			{
				if (_AddedById != value)
				{
					_AddedById = value;
					RaisePropertyChanged();
				}
			}
		}

		int _RatingOutOfFive;

		public int RatingOutOfFive
		{
			get { return _RatingOutOfFive; }
			set
			{
				if (_RatingOutOfFive != value)
				{
					_RatingOutOfFive = value;
					RaisePropertyChanged();
				}
			}
		}

		string _MainImageUrl;

		public string MainImageUrl
		{
			get { return _MainImageUrl; }
			set
			{
				if (_MainImageUrl != value)
				{
					_MainImageUrl = value;
					RaisePropertyChanged();
				}
			}
		}

		string _Category;

		public string Category
		{
			get { return _Category; }
			set
			{
				if (_Category != value)
				{
					_Category = value;
					RaisePropertyChanged();
				}
			}
		}

		string _Address;

		public string Address
		{
			get { return _Address; }
			set
			{
				if (_Address != value)
				{
					_Address = value;
					RaisePropertyChanged();
				}
			}
		}

		double _Latitude;

		public double Latitude
		{
			get { return _Latitude; }
			set
			{
				if (_Latitude != value)
				{
					_Latitude = value;
					RaisePropertyChanged();
				}
			}
		}

		double _Longitude;

		public double Longitude
		{
			get { return _Longitude; }
			set
			{
				if (_Longitude != value)
				{
					_Longitude = value;
					RaisePropertyChanged();
				}
			}
		}
	}
}
