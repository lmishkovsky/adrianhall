using System.Threading.Tasks;
using Android.Content;
using Microsoft.WindowsAzure.MobileServices;
using TaskList.Abstractions;
using TaskList.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(DroidLoginProvider))]
namespace TaskList.Droid.Services
{
	public class DroidLoginProvider : ILoginProvider
	{
		Context context;

		public void Init(Context context)
		{
			this.context = context;
		}

		public async Task LoginAsync(MobileServiceClient client)
		{
            // ANDROID SERVER FLOW AUTHENTICATION

			// this method is different from Adrian's book where they use version 3.0 of the client
			// Version 4.0.1 did not work for me either
			// this working version is 4.0.0
			// the method below accepts 3 parameters 
			// the 3rd parameter is specified in azure
			// ALLOWED EXTERNAL REDIRECT URLS: appname://easyauth.callback
			// We also need to specify this parameter in the AndroidManifest.xml file:
			// <data android:scheme="appname" android:host="easyauth.callback" /> 
			await client.LoginAsync(context, "facebook", "appname");
		}
	}   
}