using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Stores.Account;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IAccountStore _accountService;

        public string ProfileImage { get; set; }
        public string Login { get; set; }
        public string FullName { get; set; }
        public string Passport { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Birthdate { get; set; }

        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }

        public ICommand EditProfileCommand { get; }
        public ICommand LogoutCommand { get; }

        public ProfileViewModel()
        {
            _accountService = DependencyService.Get<IAccountStore>();

            EditProfileCommand = new Command(async () => await OnEditProfileAsync());
            LogoutCommand = new Command(async () => await OnLogoutAsync());
        }

        public async Task LoadProfileDataAsync()
        {
            var account = await _accountService.GetAccountAsync();

            if (account is null)
            {
                return;
            }

            Login = account.Login;
            FullName = $"{account.FirstName} {account.LastName}";
            Passport = account.Passport;
            PhoneNumber = account.PhoneNumber;
            Email = account.Email;
            Address = account.Address;
            Birthdate = account.Birthdate.ToString("dd MMMM, yyyy");

            OnPropertyChanged(nameof(ProfileImage));

            OnPropertyChanged(nameof(Login));
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Passport));
            OnPropertyChanged(nameof(PhoneNumber));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(Birthdate));

            OnPropertyChanged(nameof(VehicleMake));
            OnPropertyChanged(nameof(VehicleModel));
            OnPropertyChanged(nameof(VehicleYear));
        }

        private async Task OnEditProfileAsync()
        {
            await _navigationService.NavigateToAsync(NavigationPageType.EditProfile);
        }

        private async Task OnLogoutAsync()
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
