using System;
using Microsoft.Azure.Mobile.Server;

namespace Backend.DataObjects
{
    /// <summary>
    /// Todo item.
    /// </summary>
    public class TodoItem : EntityData
    {
        public string Text { get; set; }
        public bool Complete { get; set; }
    }
}
