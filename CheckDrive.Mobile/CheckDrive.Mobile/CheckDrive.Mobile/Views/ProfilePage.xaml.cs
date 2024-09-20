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

        protected override void OnAppearing()
        {
            var vm = BindingContext as ProfileViewModel;
            vm?.LoadProfileData();

            base.OnAppearing();
        }
    }
}