using System;
using Xamarin.Forms;

namespace MyFavSpots.Shared
{
	public class FavSpotView : InfoView
	{
		public FavSpotView() : base()
		{
			SetBindingNames("Name", "Description", "MainImageUrl");
		}
	}

	public class FavSpotCell : BaseInfoCell
	{
		public FavSpotCell() : base()
		{
			SetBindingNames("Name", "Description", "MainImageUrl");
		}
	}
}
