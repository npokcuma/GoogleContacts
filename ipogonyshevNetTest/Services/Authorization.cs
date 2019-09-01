using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;

namespace ipogonyshevNetTest.Services
{
	internal static class Authorization
	{
		/// <summary>
		/// For authorization, you must obtain ClientId and ClientSecret in advance for our application.
		/// We will use this to get a token. It is necessary to work on the API with Google.
		/// </summary>
		private static readonly ClientSecrets ClientSecrets = new ClientSecrets
		{
			ClientId = "1062889216556-s7mbmuqve920v01a0c48d4l9atfgde2b.apps.googleusercontent.com",
			ClientSecret = "pgKMMKqoItVymM2M3XP0p3HL"
		};

		public static PeopleServiceService GetGooglePeopleService()
		{
			var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
				ClientSecrets,
				new[] { "profile", "https://www.google.com/m8/feeds/contacts/default/full" },
				"me",
				CancellationToken.None).Result;

			var service = new PeopleServiceService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "NetTest",
			});

			return service;
		}
	}
}
