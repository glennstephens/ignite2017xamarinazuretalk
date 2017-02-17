using Microsoft.WindowsAzure.MobileServices;
using MyFavSpots.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MyFavSpots
{
	public partial class App : Application
	{
		public static ISpotsDataService Data;

		public static MobileServiceClient client;
		public static MobileServiceUser AuthenticatedUser;

		public App()
		{
			InitializeComponent();

			client = new MobileServiceClient(MobileServiceClientConstants.MyFavSpotAzureUrl);

			Data = new AzureSqlSpotsDataService(client);

			// Lets start with the Data Aspects of the App
			if (AuthStorage.HasLoggedIn)
			{
				// Automatically load the credentials if we have them
				AuthStorage.LoadSavedUserDetails(client);

				App.NavigateToSearchPage();
			}
			else {
				App.NavigateToSigninPage();
				MainPage = new SigninPage(client);
			}
		}

		public static void NavigateToSearchPage()
		{
			Current.MainPage = new NavigationPage(new DisplaySpotsPage(Data));
		}

		public static void NavigateToSigninPage()
		{
			Current.MainPage = new SigninPage(client);
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
