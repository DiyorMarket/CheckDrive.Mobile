using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class PersonalAccountViewModel : BaseViewModel
    {
        public ICommand LogOutProfile { get; }

        private string _fullName;
        public string FullName
        {

            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }
        private string _phoneNumber;
        public string PhoneNumber
        {

            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }
        private string _login;
        public string Login
        {

            get => _login;
            set => SetProperty(ref _login, value);
        }

        public PersonalAccountViewModel( )
        {
            LogOutProfile = new Command(NavigationLoginPage);
            GetDriverData();
        }

        private async void  GetDriverData()
        {
            IsBusy = true;

            await Task.Run(() =>
            {
                var _driver = LocalStorage.GetAccount();
                FullName = $"{_driver.FirstName} {_driver.LastName}";
                PhoneNumber = _driver.PhoneNumber;
                Login = _driver.Login;
            });

            IsBusy = false;
        }

        private void NavigationLoginPage()
        {
            LocalStorage.RemoveAllAcoountData();
            Application.Current.MainPage = new LoginPage();
        }
    }
}
