using System;
using System.Threading.Tasks;

namespace TaskList.Abstractions
{
    /// <summary>
    /// IC loud service.
    /// </summary>
    public interface ICloudService
    {
        ICloudTable<T> GetTable<T>() where T : TableData;

        Task LoginAsync();
    }
}
