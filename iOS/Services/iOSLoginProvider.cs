using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook.LoginKit;
using Foundation;
//using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using TaskList.Abstractions;
using TaskList.Helpers;
using TaskList.iOS.Services;
using UIKit;
using Xamarin.Auth;

[assembly: Xamarin.Forms.Dependency(typeof(iOSLoginProvider))]
namespace TaskList.iOS.Services
{
	public class iOSLoginProvider : ILoginProvider
	{
        public UIViewController RootView => UIApplication.SharedApplication.KeyWindow.RootViewController;
		
        /// <summary>
        /// Logins the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="client">Client.</param>
		// public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client)
        public async Task LoginAsync(MobileServiceClient client)
		{
			var accessToken = await LoginFacebookAsync();
			var zumoPayload = new JObject();
			zumoPayload["access_token"] = accessToken;
            MobileServiceUser user = await client.LoginAsync("facebook", zumoPayload);		
		}		

		#region Facebook Client Flow
		private TaskCompletionSource<string> fbtcs;

		public async Task<string> LoginFacebookAsync()
		{
			fbtcs = new TaskCompletionSource<string>();
			var loginManager = new LoginManager();

			loginManager.LogInWithReadPermissions(new[] { "public_profile" }, RootView, LoginTokenHandler);
			return await fbtcs.Task;
		}

		private void LoginTokenHandler(LoginManagerLoginResult loginResult, NSError error)
		{
			if (loginResult.Token != null)
			{
				fbtcs.TrySetResult(loginResult.Token.TokenString);
			}
			else
			{
				fbtcs.TrySetException(new Exception("Facebook Client Flow Login Failed"));
			}
		}
		#endregion
	}
}