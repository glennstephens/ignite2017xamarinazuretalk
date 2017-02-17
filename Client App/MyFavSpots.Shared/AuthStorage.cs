using System;
using Plugin.SecureStorage;
using Microsoft.WindowsAzure.MobileServices;

namespace MyFavSpots
{
	public class AuthStorage
	{
		const string userIdKey = ":UserId";
		const string tokenKey = ":Token";

		public static bool HasLoggedIn
		{
			get
			{
				return (CrossSecureStorage.Current.HasKey(userIdKey)
				        && CrossSecureStorage.Current.HasKey(tokenKey));
			}
		}

		public static void LoadSavedUserDetails(MobileServiceClient client)
		{
			if (!HasLoggedIn)
				throw new ApplicationException("You do not have saved credentials");
			
			string userId = CrossSecureStorage.Current.GetValue(userIdKey);
			string token = CrossSecureStorage.Current.GetValue(tokenKey);

			client.CurrentUser = new MobileServiceUser(userId)
			{
				MobileServiceAuthenticationToken = token
			};

			App.AuthenticatedUser = client.CurrentUser;
		}

		public static void SaveUserDetails()
		{
			CrossSecureStorage.Current.SetValue(userIdKey, App.AuthenticatedUser.UserId);
			CrossSecureStorage.Current.SetValue(tokenKey, App.AuthenticatedUser.MobileServiceAuthenticationToken);
		}
	}
}
