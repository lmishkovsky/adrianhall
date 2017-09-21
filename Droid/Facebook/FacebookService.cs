using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Facebook;
using Xamarin.Facebook.Internal;
using Xamarin.Facebook.Login;
using TaskList.Droid.Facebook;
using TaskList.Droid;

//[assembly: Xamarin.Forms.Dependency(typeof(FacebookService))]
//[assembly: Permission(Name = Android.Manifest.Permission.Internet)]
//[assembly: Permission(Name = Android.Manifest.Permission.WriteExternalStorage)]
//[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/facebook_app_id")]
//[assembly: MetaData("com.facebook.sdk.ApplicationName", Value = "@string/facebook_app_name")]

namespace FacebookHelper
{
	/// <summary>
	/// Facebook service for Android
	/// </summary>
	public class FacebookService 
	{
		/// <summary>
		/// The read permissions
		/// </summary>
		readonly List<string> _readPermissions = new List<string>
		{
			//"publish_stream,public_profile,email,user_about_me,user_friends"
            "public_profile,email"
		};

		/// <summary>
		/// Logouts this instance.
		/// </summary>
		public void Logout()
		{
			LoginManager.Instance.LogOut();
		}

		/// <summary>
		/// Signs the into facebook.
		/// </summary>
		/// <returns>Task&lt;FacebookLoginResult&gt;.</returns>
		public async Task<FacebookLoginResult> SignIntoFacebook()
		{
			var login = new FacebookLoginResult();

			if (AccessToken.CurrentAccessToken == null ||
				(AccessToken.CurrentAccessToken != null && AccessToken.CurrentAccessToken.IsExpired))
			{
				// LoginManager.Instance.LogOut();

				var taskCompletionSource = new TaskCompletionSource<FacebookLoginResult>();
				var activity = Xamarin.Forms.Forms.Context as MainActivity;
				var loginCallback = new FacebookCallback<LoginResult>
				{
					HandleSuccess = loginResult =>
					{
						UpdateFacebookResult(login);
						taskCompletionSource.SetResult(login);
					},
					HandleCancel = () =>
					{
						System.Diagnostics.Debug.WriteLine("fbcancel");
						UpdateFacebookResult(login, true, false);
						taskCompletionSource.SetResult(login);
					},
					HandleError = loginError =>
					{
						System.Diagnostics.Debug.WriteLine("fberror");
						login.Error = loginError.Message;
						UpdateFacebookResult(login, true, true);
						taskCompletionSource.SetResult(login);
					}
				};

				if (activity == null) return null;

				//LoginManager.Instance.RegisterCallback(activity.CallbackManager, loginCallback);

				LoginManager.Instance.LogInWithReadPermissions(activity, _readPermissions);

				return await taskCompletionSource.Task;
			}

			UpdateFacebookResult(login);

			return login;
		}

		/// <summary>
		/// Updates the model.
		/// </summary>
		/// <param name="login">The login.</param>
		/// <param name="iscancelled">if set to <c>true</c> [iscancelled].</param>
		/// <param name="iserror">if set to <c>true</c> [iserror].</param>
		private void UpdateFacebookResult(FacebookLoginResult login, bool iscancelled = false, bool iserror = false)
		{
			var token = AccessToken.CurrentAccessToken != null;
			if (token)
			{
				login.Token = AccessToken.CurrentAccessToken?.Token;
				login.ExpirationDate = AccessToken.CurrentAccessToken != null
					? JavaToCsharpDateTime(AccessToken.CurrentAccessToken?.Expires.Time)
					: null;
				login.UserId = AccessToken.CurrentAccessToken?.UserId;
				login.IsCancelled = iscancelled;
			}
			else
			{
				login.IsCancelled = iscancelled;
			}
		}

		/// <summary>
		/// Javas to csharp date time.
		/// </summary>
		/// <param name="longTimeMillis">The unix time millis.</param>
		/// <returns>System.Nullable&lt;DateTime&gt;.</returns>
		public DateTime? JavaToCsharpDateTime(long? longTimeMillis)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			if (longTimeMillis != null) return epoch.AddMilliseconds(longTimeMillis.Value);
			return null;
		}

        /*
		/// <summary>
		/// Gets the profile information.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns>Task&lt;UserProfile&gt;.</returns>
		public async Task<UserProfile> GetProfileInfo(string token)
		{
			UserProfile userProfile;
			var taskCompletionSource = new TaskCompletionSource<UserProfile>();
			var parameters = new Bundle();
			parameters.PutString("fields", "name,email,picture.type(large)");
			var client = new WebClient();
			var response = new Response()
			{
				HandleSuccess = (respose) =>
				{
					userProfile = new UserProfile
					{
						Name = respose.JSONObject.GetString("name"),
						Email = respose.JSONObject.GetString("email"),
						ProfilePictureUrl =
							respose.JSONObject.GetJSONObject("picture").GetJSONObject("data").GetString("url")
					};

					var pictureUrl = respose.JSONObject.GetJSONObject("picture").GetJSONObject("data").GetString("url");

					// Download user profile picture
					var pictureData = client.DownloadData(pictureUrl);
					userProfile.ProfilePicture = pictureData;


					taskCompletionSource.SetResult(userProfile);
				}
			};


			var graphRequest = new GraphRequest(AccessToken.CurrentAccessToken,
				"/" + AccessToken.CurrentAccessToken.UserId, parameters, HttpMethod.Get, response);
			graphRequest.ExecuteAsync();
			return await taskCompletionSource.Task;
		}
        */

		public async Task<bool> PostToFacebook(string statusUpdate, byte[] media)
		{
			if (AccessToken.CurrentAccessToken == null || string.IsNullOrEmpty(AccessToken.CurrentAccessToken.UserId))
				return false;
			var taskCompletionSource = new TaskCompletionSource<bool>();
			var parameters = new Bundle();
			parameters.PutString("message", statusUpdate);
			parameters.PutByteArray("picture", media);

			var response = new Response()
			{
				HandleSuccess = (respose) => { taskCompletionSource.SetResult(true); }
			};

			var graphRequest = new GraphRequest(AccessToken.CurrentAccessToken,
				media != null ? "/me/photos" : "/me/feed",
				parameters,
				HttpMethod.Post, response);
			graphRequest.ExecuteAsync();
			return await taskCompletionSource.Task;
		}

		const int _requestLimit = 5000;

        /*
		public async Task<List<FacebookFriend>> GetFriendsList()
		{
			// UserProfile userProfile;
			if (AccessToken.CurrentAccessToken == null || string.IsNullOrEmpty(AccessToken.CurrentAccessToken.UserId))
				return null;
			var facebookFriend = new List<FacebookFriend>();
			var taskCompletionSource = new TaskCompletionSource<List<FacebookFriend>>();
			var parameters = new Bundle();
			parameters.PutString("fields", "id, name,picture");
			parameters.PutString("limit", _requestLimit.ToString());

			var response = new Response()
			{
				HandleSuccess = (respose) =>
				{
					var data = respose.JSONObject.GetJSONArray("data");

					for (int i = 0; i < data.Length(); i++)
					{
						var jsonobject = data.GetJSONObject(i);
						var friendId = jsonobject.GetString("id");

						var fbFriend = new FacebookFriend()
						{
							Id = friendId,
							Name = jsonobject.GetString("name"),
							PictureUrl = $"https://graph.facebook.com/{friendId}/picture?type=normal"
						};

						facebookFriend.Add(fbFriend);
					}
					taskCompletionSource.SetResult(facebookFriend);
				}
			};


			var graphRequest = new GraphRequest(AccessToken.CurrentAccessToken,
				"/" + AccessToken.CurrentAccessToken.UserId + "/friends",
				null,
				HttpMethod.Get, response);
			graphRequest.ExecuteAsync();
			return await taskCompletionSource.Task;
		}
		*/

		/// <summary>
		/// Class Response.
		/// </summary>
		class Response : Java.Lang.Object, GraphRequest.ICallback
		{
			/// <summary>
			/// Gets or sets the handle success.
			/// </summary>
			/// <value>The handle success.</value>
			public Action<GraphResponse> HandleSuccess { get; set; }

			/// <summary>
			/// Called when [completed].
			/// </summary>
			/// <param name="response"></param>
			public void OnCompleted(GraphResponse response)
			{
				var c = HandleSuccess;
				c.Invoke(response);
			}
		}

		/// <summary>
		/// Class FacebookCallback.
		/// </summary>
		/// <typeparam name="TResult">The type of the t result.</typeparam>
		private class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
		{
			/// <summary>
			/// Gets or sets the handle cancel.
			/// </summary>
			/// <value>The handle cancel.</value>
			public Action HandleCancel { get; set; }

			/// <summary>
			/// Gets or sets the handle error.
			/// </summary>
			/// <value>The handle error.</value>
			public Action<FacebookException> HandleError { get; set; }

			/// <summary>
			/// Gets or sets the handle success.
			/// </summary>
			/// <value>The handle success.</value>
			public Action<TResult> HandleSuccess { get; set; }

			/// <summary>
			/// Called when [cancel].
			/// </summary>
			public void OnCancel()
			{
				var c = HandleCancel;
				c?.Invoke();
			}

			/// <summary>
			/// Called when [error].
			/// </summary>
			/// <param name="error">The error.</param>
			public void OnError(FacebookException error)
			{
				var c = HandleError;
				c?.Invoke(error);
			}

			/// <summary>
			/// Called when [success].
			/// </summary>
			/// <param name="result">The result.</param>
			public void OnSuccess(Java.Lang.Object result)
			{
				var c = HandleSuccess;
				c?.Invoke(result.JavaCast<TResult>());
			}
		}
	}
}