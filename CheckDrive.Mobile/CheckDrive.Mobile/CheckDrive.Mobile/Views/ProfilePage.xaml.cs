using CheckDrive.Mobile.ViewModels;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Views
{
    public partial class ProfilePage : ContentPage
    {
        private readonly ProfileViewModel _viewModel;

        public ProfilePage()
        {
            InitializeComponent();

            _viewModel = new ProfileViewModel();

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.LoadProfileDataAsync();

            base.OnAppearing();
        }
    }
}