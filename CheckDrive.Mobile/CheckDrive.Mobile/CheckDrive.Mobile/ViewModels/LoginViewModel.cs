using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Stores.Account;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAccountStore _accountDataStore;

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

            LoginCommand = new Command(async () => await ExecuteLoginCommand());
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
        }

        private async Task ExecuteLoginCommand()
        {
            if (!ValidateInput())
            {
                return;
            }

            IsBusy = true;
            IsLoginButtonVisible = false;
            IsErrorVisible = false;

            try
            {
                await _accountDataStore.LoginAsync(Login, Password);
                var role = await _accountDataStore.GetUserRoleAsync();

                RegisterRoutes(role);

                await _navigationService.NavigateToAsync(NavigationPageType.Home);
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

        private static void RegisterRoutes(string role)
        {
            (Application.Current as App)?.RegisterRoutes(role);
        }
    }
}