using TaskList.Abstractions;

namespace TaskList.Models
{
    /// <summary>
    /// Todo item.
    /// </summary>
	public class TodoItem : TableData
	{
		public string Text { get; set; }
		public bool Complete { get; set; }
	}
}