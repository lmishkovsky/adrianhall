using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TaskList.Abstractions;
using TaskList.Helpers;
using Xamarin.Forms;

namespace TaskList.ViewModels
{
	public class EntryPageViewModel : BaseViewModel
	{
		public EntryPageViewModel()
		{
			Title = "Task List";
		}

		Command loginCmd;
		public Command LoginCommand => loginCmd ?? (loginCmd = new Command(async () => await ExecuteLoginCommand()));

        /// <summary>
        /// Executes the login command.
        /// </summary>
        /// <returns>The login command.</returns>
		async Task ExecuteLoginCommand()
		{
			if (IsBusy)
				return;
			IsBusy = true;

			try
			{
				var cloudService = ServiceLocator.Instance.Resolve<ICloudService>();
				await cloudService.LoginAsync();

                Application.Current.MainPage = new NavigationPage(new Views.TaskList());
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[Login] Error = {ex.Message}");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
