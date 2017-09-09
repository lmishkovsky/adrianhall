using System;
using System.Collections.Generic;
using TaskList.ViewModels;
using Xamarin.Forms;

namespace TaskList.Views
{
    public partial class TaskList : ContentPage
    {
        public TaskList()
        {
            InitializeComponent();

            BindingContext = new TaskListViewModel();
        }
    }
}
