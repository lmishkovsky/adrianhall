using TaskList.Abstractions;
using TaskList.Helpers;
using TaskList.Services;
using Xamarin.Forms;

namespace TaskList
{
	public partial class App : Application
	{
		public static ICloudService CloudService { get; set; }

		public App()
		{
			//CloudService = new AzureCloudService();

            ServiceLocator.Instance.Add<ICloudService, AzureCloudService>();

            MainPage = new NavigationPage(new Views.EntryPage());
		}

		// There are life cycle methods here...
	}
}