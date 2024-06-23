using CheckDrive.ApiContracts.Driver;
using CheckDrive.Mobile.Services;
using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Drivers;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IDriverDataStore _driverDataStore;

        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ToggleLoginVisibilityCommand { get; }
        public ICommand LoginCommand { get; }

        private DriverDto Account { get; set; }

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

        public LoginViewModel(IAccountDataStore accountDataStore, IDriverDataStore driverDataStore)
        {
            _accountDataStore = accountDataStore;
            _driverDataStore = driverDataStore;
            LoginCommand = new Command(async (obj) => await OnLoginClicked());
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
            ToggleLoginVisibilityCommand = new Command(ToggleLoginVisibility);
        }

        public async Task OnLoginClicked()
        {
            IsBusy = true;

            if (string.IsNullOrWhiteSpace(Login))
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("Error", "Login cannot be empty", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("Error", "Password cannot be empty", "OK");
                return;
            }

            try
            {
                var isSuccess = await CheckingDriverLogin();

                if (isSuccess)
                {
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "Please check your credentials and try again.", "OK");
                }
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Login Failed", "An error occurred. Please try again.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<bool> CheckingDriverLogin()
        {
            var token = await _accountDataStore.CreateTokenAsync(Login, Password);

            if (token != null)
            {
                DataService.SaveToken(token);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var accountId = int.Parse(jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var driverResponse = await _driverDataStore.GetDriversAsync(accountId);
            var driver = driverResponse.Data.ToList().FirstOrDefault();

            if (driver != null)
            {
                Account = driver;
                DataService.SaveAccount(driver);
                return true;
            }

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
