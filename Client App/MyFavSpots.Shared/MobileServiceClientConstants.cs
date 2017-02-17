using System;

namespace MyFavSpots.Shared
{
	public class MobileServiceClientConstants
	{
		// Constants for the Mobile Service
		// Make sure you use https when using Authentication with Facebook
		public const string MyFavSpotAzureUrl = "https://YourAzureMobileServiceApp.azurewebsites.net/";

		// Constant for the Notification Hub
		public const string NotificationHubName = "YourNotificationHubName";

		// Constants for the Blob storage	
		public const string CloudStorageConnectionString = "";
		public const string CloudStorageContainerName = "mycontainername";
		public const string CloudStorageUrlPath = "https://mycontainername.blob.core.windows.net/spotphotos/{0}";

		// Constants used for Cognative Services
		public const string CognativeServicesVisionApiKey = "";
	}
}
