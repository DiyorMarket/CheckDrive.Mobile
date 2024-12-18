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

            LoginInput.Focus();
        }

        private void UserName_Completed(object sender, System.EventArgs e)
        {
            PasswordInput.Focus();
        }

        private void Password_Completed(object sender, System.EventArgs e)
        {
            LoginButton.Focus();
        }
    }
}