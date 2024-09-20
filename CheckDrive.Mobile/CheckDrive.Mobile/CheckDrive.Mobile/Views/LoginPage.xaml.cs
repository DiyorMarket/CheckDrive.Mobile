using CheckDrive.Mobile.ViewModels;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();

            _viewModel = new LoginViewModel();

            BindingContext = _viewModel;
        }
    }
}