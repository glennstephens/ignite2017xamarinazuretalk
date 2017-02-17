using System;
using System.Threading.Tasks;
using MyFavSpots.Shared;
using Acr.UserDialogs;
using Xamarin.Forms;
using Android.App;

[assembly: Dependency(typeof(MyFavSpots.Droid.ShowInProgressDialogDroid))]

namespace MyFavSpots.Droid
{
	public class ShowInProgressDialogDroid : IShowInProgressDialog
	{
		Acr.UserDialogs.ProgressDialog dlg;

		public void Show(string message)
		{
			dlg = new Acr.UserDialogs.ProgressDialog(new ProgressDialogConfig()
			{
				AutoShow = true,
				Title = "Saving your awesome new spot...",
				IsDeterministic = false
			}, Forms.Context as Activity);

			dlg.Show();
		}

		public void Hide()
		{
			dlg.Hide();
			dlg = null;
		}
	}
}
