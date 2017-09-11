using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace TaskList.Abstractions
{
    /// <summary>
    /// Login provider.
    /// </summary>
	public interface ILoginProvider
	{
		Task LoginAsync(MobileServiceClient client);
	}
}
