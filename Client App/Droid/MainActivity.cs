using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Android.Util;
using Plugin.SecureStorage;
using Acr.UserDialogs;
using FFImageLoading.Forms.Droid;

namespace MyFavSpots.Droid
{
	[Activity(Label = "MyFavSpots.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			SecureStorageImplementation.StoragePassword = Build.Id;

			UserDialogs.Init(this);

			// Initialize our Gcm Service Hub
			FavSpotsGcmService.Initialize(this);

			// Register for GCM
			FavSpotsGcmService.Register(this);

			global::Xamarin.Forms.Forms.Init(this, bundle);
			Xamarin.FormsMaps.Init(this, bundle);

			CachedImageRenderer.Init();

			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			LoadApplication(new App());
		}
	}
}
