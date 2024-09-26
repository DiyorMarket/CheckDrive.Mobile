using CheckDrive.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfile : ContentPage
    {
        private readonly ProfileViewModel _viewModel;
        public EditProfile()
        {
            InitializeComponent();

            _viewModel = new ProfileViewModel();

            BindingContext = _viewModel;
        }
        protected override async void OnAppearing()
        {
            if (BindingContext is ProfileViewModel vm)
            {
                await vm.LoadProfileDataAsync();
            }

            base.OnAppearing();
        }
    }
}