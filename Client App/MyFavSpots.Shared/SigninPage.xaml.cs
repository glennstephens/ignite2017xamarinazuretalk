using System;
using Microsoft.WindowsAzure.MobileServices;

using Xamarin.Forms;
#if __ANDROID__
#elif __IOS__
using UIKit;
#endif

namespace MyFavSpots
{
	public partial class SigninPage : ContentPage
	{
		readonly MobileServiceClient client;

		public SigninPage(MobileServiceClient client)
		{
			this.client = client;
			InitializeComponent();
		}

		async void PerformLogin(object sender, System.EventArgs e)
		{
			// Check that we don't have a saved login
			if (AuthStorage.HasLoggedIn)
			{
				// Automatically load the credentials if we have
				AuthStorage.LoadSavedUserDetails(client);

				App.NavigateToSearchPage();
				return;
			}

			// Perform the login
	#if __ANDROID__
				App.AuthenticatedUser = await client.LoginAsync(Forms.Context, 
			                                                    MobileServiceAuthenticationProvider.Facebook);
			#elif __IOS__
				App.AuthenticatedUser = await client.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController,
														   MobileServiceAuthenticationProvider.Facebook);
#endif

			// Save if we logged in
			if (App.AuthenticatedUser != null)
			{
				AuthStorage.SaveUserDetails();
				App.NavigateToSearchPage();
			}
		}
	}
}
