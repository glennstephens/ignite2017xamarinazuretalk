using System;
using System.Threading.Tasks;
using MyFavSpots.Shared;
using Acr.UserDialogs;
using Xamarin.Forms;

[assembly: Dependency(typeof(MyFavSpots.iOS.ShowInProgressDialogDroid))]

namespace MyFavSpots.iOS
{
	public class ShowInProgressDialogDroid : IShowInProgressDialog
	{
		ProgressDialog dlg;

		public void Show(string message)
		{
			dlg = new ProgressDialog(new ProgressDialogConfig()
			{
				AutoShow = true,
				Title = message,
				IsDeterministic = false
			});

			dlg.Show();
		}

		public void Hide()
		{
			dlg.Hide();
			dlg = null;
		}
	}
}
