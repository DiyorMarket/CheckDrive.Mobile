using CheckDrive.Mobile.Models.Account;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Stores.Account;
using CheckDrive.Mobile.Stores.Auth;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IAuthStore _authStore;
        private readonly IAccountStore _accountService;

        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Passport { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Birthdate { get; set; }
        public string Position { get; set; }
        public string AssignedCar { get; set; }

        public ICommand LogoutCommand { get; }

        public ProfileViewModel()
        {
            _accountService = DependencyService.Get<IAccountStore>();
            _authStore = DependencyService.Get<IAuthStore>();

            LogoutCommand = new Command(async () => await OnLogoutAsync());
        }

        public async Task LoadProfileDataAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var accountId = await _accountService.GetAccountIdAsync();
                var account = await _accountService.GetAccountAsync(accountId);

                if (account is null)
                {
                    return;
                }

                UserName = account.UserName;
                FullName = $"{account.FirstName} {account.LastName} {account.Patronymic}";
                Passport = account.Passport;
                PhoneNumber = account.PhoneNumber;
                Email = account.Email;
                Address = account.Address;
                Birthdate = account.Birthdate.ToString("dd MMMM, yyyy");
                Position = account.Position.ToString();
                AssignedCar = "Qora Gentra (AA707B)";

                OnPropertyChanged(nameof(UserName));
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(Passport));
                OnPropertyChanged(nameof(PhoneNumber));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Address));
                OnPropertyChanged(nameof(Birthdate));
                OnPropertyChanged(nameof(Position));
                OnPropertyChanged(nameof(AssignedCar));
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync($"Profilni yuklashda xato ro'y berdi.", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnEditProfileAsync()
        {
            await _navigationService.NavigateToAsync(NavigationPageType.EditProfile);
        }

        private async Task SaveEditsAsync()
        {
            var updatedAccount = GetAccount();

            try
            {
                var result = await _accountService.UpdateAccountAsync(updatedAccount);

                if (result == null)
                {
                    throw new Exception("Account update result is null.");
                }

                await _navigationService.NavigateToAsync(NavigationPageType.Profile);
                await Application.Current.MainPage.DisplayAlert("Success", "Profil muvafaqiyatli tarzda o'zgartirildi.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Shaxsiy ma'lumotni yangilashda kutilmagan muammo yuzaga keldi. Iltimos, qayta urunib ko'ring yoki texnik yordam bilan bog'laning.", "OK");
            }
        }

        private async Task OnLogoutAsync()
        {
            var confirmed = await Application.Current.MainPage.DisplayAlert("Amalni tasdiqlang", "Hisobingizdan chiqishni istaysizmi?", "Ha", "Yo'q");

            if (confirmed)
            {
                _authStore.Logout();

                await _navigationService.NavigateToAsync(NavigationPageType.Login);
            }
        }

        private async Task OnBack()
        {
            await _navigationService.NavigateToAsync(NavigationPageType.Profile);
        }

        private AccountDto GetAccount()
        {
            return new AccountDto
            {
                UserName = UserName,
                FirstName = FullName.Split(' ')[0],
                LastName = FullName.Split(' ').Length > 1 ? FullName.Split(' ')[1] : "",
                Passport = Passport,
                PhoneNumber = PhoneNumber,
                Email = Email,
                Address = Address,
                Birthdate = DateTime.Parse(Birthdate),
            };
        }
    }
}
