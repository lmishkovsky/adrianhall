using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using TaskList.Abstractions;
using Xamarin.Forms;

namespace TaskList.Services
{
    /// <summary>
    /// Azure cloud service.
    /// </summary>
	public class AzureCloudService : ICloudService
	{
		MobileServiceClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskList.Services.AzureCloudService"/> class.
        /// </summary>
		public AzureCloudService()
		{
			client = new MobileServiceClient("https://adrianhall-backend.azurewebsites.net");
		}

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
		public ICloudTable<T> GetTable<T>() where T : TableData
		{
			return new AzureCloudTable<T>(client);
		}

        /// <summary>
        /// Logins the async.
        /// </summary>
        /// <returns>The async.</returns>
		public Task LoginAsync()
		{
			var loginProvider = DependencyService.Get<ILoginProvider>();
			return loginProvider.LoginAsync(client);
		}
	}
}
