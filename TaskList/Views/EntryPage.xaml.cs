using System;
using System.Collections.Generic;
using TaskList.ViewModels;
using Xamarin.Forms;

namespace TaskList.Views
{
    public partial class EntryPage : ContentPage
    {
        public EntryPage()
        {
            InitializeComponent();

            BindingContext = new EntryPageViewModel();
        }
    }
}
