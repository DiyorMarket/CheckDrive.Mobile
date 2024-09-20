using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Stores.Accounts;
using CheckDrive.Mobile.Stores.Drivers;
using CheckDrive.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            var client = new ApiClient();
            var accountDS = new AccountDataStore(client);
            var driverDS = new DriverDataStore(client);

            this.BindingContext = new LoginViewModel(accountDS, driverDS);
        }
    }
}