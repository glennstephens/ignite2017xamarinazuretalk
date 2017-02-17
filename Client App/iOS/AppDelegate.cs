using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using MyFavSpots.Shared;

using WindowsAzure.Messaging;
using FFImageLoading.Forms.Touch;

namespace MyFavSpots.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			SetupAppearance();

			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			SetupPushNotifications(app, options);
			                       
			global::Xamarin.Forms.Forms.Init();
			Xamarin.FormsMaps.Init();

			CachedImageRenderer.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

		void SetupPushNotifications(UIApplication app, NSDictionary options)
		{
			var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
				UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);

			app.RegisterUserNotificationSettings(pushSettings);
			app.RegisterForRemoteNotifications();

			ProcessNotification(options, false);
		}

		public SBNotificationHub Hub { get; set; }

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			Hub = new SBNotificationHub(MobileServiceClientIOSConstants.AzurePushConnectionString, MobileServiceClientConstants.NotificationHubName);
			Hub.UnregisterAllAsync(deviceToken, error =>
			{
				if (error != null)
				{
					LogError($"Error Unregistering: {error}");
				}

				Hub.RegisterNativeAsync(deviceToken, null, registerError =>
				{
					if (registerError != null)
					{
						LogError($"Error Registering: {registerError}");
					}
				});
			});
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			ProcessNotification(userInfo, false);
		}

		void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
		{
			if (options == null)
				return;

			var apsKey = new NSString("aps");
			if (!options.ContainsKey(apsKey))
				return;

			var aps = options.ObjectForKey(apsKey) as NSDictionary;
			var alertKey = new NSString("alert");

			var message = (aps[alertKey] as NSString).ToString();

			if (!fromFinishedLaunching)
			{
				//Manually show an alert
				if (!string.IsNullOrEmpty(message))
				{
					var avAlert = new UIAlertView("Notification", message, null, "Close", null);
					avAlert.Show();
				}
			}
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			LogError($"Failed to register for remote notifications: {error.Description}");
		}

		void LogError(object message)
		{
			Console.WriteLine(message);
		}

		void SetupAppearance()
		{
			UIFont coreFont = UIFont.FromName("Avenir-Light", 15f);
			UILabel.Appearance.Font = coreFont;
			UILabel.AppearanceWhenContainedIn(typeof(UITextField)).Font = coreFont;

			UINavigationBar.Appearance.SetTitleTextAttributes(
				new UITextAttributes()
			{
				Font = UIFont.FromName("Noteworthy-Bold", 18f)
			});

			UIBarButtonItem.Appearance.TintColor = UIColor.FromRGB(78, 81, 171);
		}
	}
}
