using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;

namespace ipogonyshevNetTest.Services
{
	internal static class Authorization
	{
		static readonly ClientSecrets secrets = new ClientSecrets()
		{
			ClientId = "1062889216556-s7mbmuqve920v01a0c48d4l9atfgde2b.apps.googleusercontent.com",
			ClientSecret = "pgKMMKqoItVymM2M3XP0p3HL"
		};

		public static PeopleServiceService GetToken()
		{
			// Create OAuth credential.
			UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
				secrets,
				new[] { "profile", "https://www.google.com/m8/feeds/contacts/default/full" },
				"me",
				CancellationToken.None).Result;

			// Create the service.
			var service = new PeopleServiceService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "NetTest",
			});
			return service;
		}
	}
}
