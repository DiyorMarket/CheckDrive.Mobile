using CheckDrive.ApiContracts.Driver;
using CheckDrive.Mobile.Services;
using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Drivers;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IDriverDataStore _driverDataStore;

        private DriverDto Account {  get; set; }

        private string _login;
        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                _isPasswordVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoginVisible;
        public bool IsLoginVisible
        {
            get => _isLoginVisible;
            set
            {
                _isLoginVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ToggleLoginVisibilityCommand { get; }

        public Command LoginCommand { get; }

        public LoginViewModel(IAccountDataStore accountDataStore, IDriverDataStore driverDataStore)
        {
            _accountDataStore = accountDataStore;
            _driverDataStore = driverDataStore;
            LoginCommand = new Command(OnLoginClicked);
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
            ToggleLoginVisibilityCommand = new Command(ToggleLoginVisibility);
        }

        private void OnLoginClicked(object obj)
        {
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(Login))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                return;
            }

            try
            {
                IsBusy = true;

                var isSuccess = CheckingDriverLogin();
                Application.Current.MainPage = new AppShell();
            }
            catch
            {
                Application.Current.MainPage.DisplayAlert("Login Failed", "Please check your credentials and try again.", "OK");
            }
            IsBusy = false;
        }

        private async Task<bool> CheckingDriverLogin()
        {
            IsBusy = true;

            var token = await _accountDataStore.CreateTokenAsync(Login, Password);

            if (token != null)
            {
                DataService.SaveToken(token);
            }

            var accountsResponse = await _accountDataStore.GetAccountsAsync(Login);
            var account = accountsResponse.Data.ToList().First();

            var driverResponse = await _driverDataStore.GetDriversAsync(account.Id);
            var driver = driverResponse.Data.ToList().First();

            if (driver != null)
            {
                Account = driver;
                DataService.SaveAccount(driver);
                return true;
            }

            IsBusy= false;

            return false;
        }

        private void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        private void ToggleLoginVisibility()
        {
            IsLoginVisible = !IsLoginVisible;
        }
    }
}
