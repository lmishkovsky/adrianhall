using System;
using System.Collections.Generic;
using TaskList.Models;
using TaskList.ViewModels;
using Xamarin.Forms;

namespace TaskList.Views
{
	public partial class TaskDetail : ContentPage
	{
		public TaskDetail(TodoItem item = null)
		{
			InitializeComponent();
			BindingContext = new TaskDetailViewModel(item);
		}
	}
}
