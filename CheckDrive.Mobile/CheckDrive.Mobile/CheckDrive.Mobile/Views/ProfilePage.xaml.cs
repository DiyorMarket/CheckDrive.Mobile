using CheckDrive.Mobile.Services.Navigation;
using CheckDrive.Mobile.Stores.Accounts;
using CheckDrive.Mobile.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();

            var accountStore = new AccountStore(new Services.ApiClient());
            var navigationService = new NavigationService();

            BindingContext = new ProfileViewModel(accountStore, navigationService);
        }

        protected override void OnAppearing()
        {
            var vm = BindingContext as ProfileViewModel;
            vm?.LoadProfileData();

            base.OnAppearing();
        }
    }
}