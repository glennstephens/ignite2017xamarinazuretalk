using System;

namespace MyFavSpots.Shared
{
	public interface IShowInProgressDialog
	{
		void Show(string message);
		void Hide();
	}
}
