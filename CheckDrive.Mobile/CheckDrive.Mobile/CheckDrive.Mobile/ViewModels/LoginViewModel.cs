using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Stores.Account;
using CheckDrive.Mobile.Stores.Auth;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthStore _authStore;
        private readonly IAccountStore _accountDataStore;

        public ICommand LoginCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }

        public string UserName { get; set; }
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
            _authStore = DependencyService.Get<IAuthStore>();
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
                var success = await _authStore.LoginAsync(UserName, Password);

                if (!success)
                {
                    ShowError("Invalid login attempt.");
                    return;
                }

                var role = await _accountDataStore.GetUserRoleAsync();

                Application.Current.MainPage = new AppShell(role);
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
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
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