using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Services.Navigation;
using CheckDrive.Mobile.Stores.Accounts;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IAccountStore _accountService;
        private readonly INavigationService _navigationService;

        public string ProfileImage { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime Birthdate { get; set; }

        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }

        public ICommand EditProfileCommand { get; }
        public ICommand LogoutCommand { get; }

        public ProfileViewModel()
        {
            _accountService = DependencyService.Get<IAccountStore>();
            _navigationService = DependencyService.Get<INavigationService>();

            EditProfileCommand = new Command(OnEditProfile);
            LogoutCommand = new Command(OnLogout);
        }

        public async Task LoadProfileDataAsync()
        {
            var account = await _accountService.GetAccountAsync();

            if (account is null)
            {
                return;
            }

            FullName = $"{account.FirstName} {account.LastName}";
            Phone = account.PhoneNumber;
            Birthdate = account.Bithdate;

            OnPropertyChanged(nameof(ProfileImage));

            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Phone));
            OnPropertyChanged(nameof(Birthdate));

            OnPropertyChanged(nameof(VehicleMake));
            OnPropertyChanged(nameof(VehicleModel));
            OnPropertyChanged(nameof(VehicleYear));
        }

        private async void OnEditProfile()
        {
            // await _navigationService.NavigateToAsync<EditProfilePage>();
        }

        private async void OnLogout()
        {
            var confirmed = await Application.Current.MainPage.DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");

            LocalStorage.ClearAll();

            if (confirmed)
            {
                await _accountService.LogoutAsync();

                await _navigationService.NavigateToAsync(NavigationPageType.Login);
            }
        }
    }
}
