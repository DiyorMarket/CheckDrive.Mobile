using CheckDrive.Mobile.Stores.Accounts;
using CheckDrive.Mobile.Stores.Drivers;
using CheckDrive.Mobile.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAccountStore _accountDataStore;
        private readonly IDriverDataStore _driverDataStore;

        public ICommand LoginCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }

        public string Login { get; set; }
        public string Password { get; set; }

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set => SetProperty(ref _isPasswordVisible, value);
        }

        private bool _isLoginButtonVisible = true;
        public bool IsLoginButtonVisible
        {
            get => _isLoginButtonVisible;
            set => SetProperty(ref _isLoginButtonVisible, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _isErrorVisible;
        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set => SetProperty(ref _isErrorVisible, value);
        }

        public LoginViewModel()
        {
            _accountDataStore = DependencyService.Get<IAccountStore>();
            _driverDataStore = DependencyService.Get<IDriverDataStore>();

            LoginCommand = new Command(async () => await ExecuteLoginCommand());
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
        }

        private async Task ExecuteLoginCommand()
        {
            if (!ValidateInput())
            {
                return;
            }

            IsLoginButtonVisible = false;
            IsBusy = true;
            IsErrorVisible = false;

            try
            {
                await _accountDataStore.LoginAsync(Login, Password);

                await Shell.Current.GoToAsync(nameof(ProfilePage), true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Error: {ex.Message}");
                ShowError("An error occurred. Please try again.");
            }
            finally
            {
                IsBusy = false;
                IsLoginButtonVisible = true;
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                ShowError("Foydalanuvchi nomini va parolni kiriting.");
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            IsErrorVisible = true;
        }

        private void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }
    }
}