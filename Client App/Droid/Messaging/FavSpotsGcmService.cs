using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Widget;
using Gcm.Client;
using MyFavSpots.Shared;
using WindowsAzure.Messaging;

[assembly: UsesPermission(Android.Manifest.Permission.ReceiveBootCompleted)]

namespace MyFavSpots.Droid
{
	[BroadcastReceiver(Permission = Constants.PERMISSION_GCM_INTENTS)]
	[IntentFilter(new[] { Intent.ActionBootCompleted })] // Allow GCM on boot and when app is closed   
	[IntentFilter(new string[] { Constants.INTENT_FROM_GCM_MESSAGE },
	Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter(new string[] { Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK },
	Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter(new string[] { Constants.INTENT_FROM_GCM_LIBRARY_RETRY },
	Categories = new string[] { "@PACKAGE_NAME@" })]
	public class SampleGcmBroadcastReceiver : GcmBroadcastReceiverBase<FavSpotsGcmService>
	{
		//IMPORTANT: Change this to your own Sender ID!
		//The SENDER_ID is your Google API Console App Project Number
		// For Firebase Messaging, this will be the Legacy Sender ID
		public static string[] SENDER_IDS = { MobileServiceClientDroidConstants.GoogleGCMSenderID };
	}

	[Service]
	public class FavSpotsGcmService : GcmServiceBase
	{
		static NotificationHub hub;

		public static void Initialize(Context context)
		{
			// Call this from our main activity
			var cs = ConnectionString.CreateUsingSharedAccessKeyWithListenAccess(
				new Java.Net.URI(MobileServiceClientDroidConstants.AzurePushConnectionString),
				MobileServiceClientDroidConstants.AzurePushNotificationKey);

			hub = new NotificationHub(MobileServiceClientConstants.NotificationHubName, cs, context);
		}

		public static void Register(Context Context)
		{
			// Makes this easier to call from our Activity
			GcmClient.Register(Context, SampleGcmBroadcastReceiver.SENDER_IDS);
		}

		public FavSpotsGcmService() : base(SampleGcmBroadcastReceiver.SENDER_IDS)
		{
			
		}

		protected override void OnRegistered(Context context, string registrationId)
		{
			//Receive registration Id for sending GCM Push Notifications to
			if (hub != null)
			{
				// Supply additional tags to the Register method if required
				hub.Register(registrationId);
			}
		}

		protected override void OnUnRegistered(Context context, string registrationId)
		{
			if (hub != null)
				hub.Unregister();
		}

		protected override void OnMessage(Context context, Intent intent)
		{
			Console.WriteLine("Received Notification");

			// Push Notification arrived - let's send a notification
			if (intent != null || intent.Extras != null)
			{
				var message = intent.Extras.GetString("message", "");
				if (message != null)
				{
					intent.AddFlags(ActivityFlags.ClearTop);
					var pendingIntent = PendingIntent.GetActivity(
						this, 0, new Intent(this, typeof(MainActivity)), PendingIntentFlags.OneShot);

					var notificationBuilder = new Notification.Builder(this)
						.SetSmallIcon(Resource.Drawable.MainIcon)
						.SetContentTitle("Notification Received")
						.SetContentText(message)
                        .SetAutoCancel(true)
					    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
					    .SetContentIntent(pendingIntent);

					var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
					notificationManager.Notify(0, notificationBuilder.Build());
				}
			}
		}

		protected override bool OnRecoverableError(Context context, string errorId)
		{
			// Some recoverable error happened
			return true;
		}

		protected override void OnError(Context context, string errorId)
		{
			// Some more serious error happened
		}
	}
}
