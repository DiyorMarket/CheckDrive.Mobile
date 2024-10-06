using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Views.Doctor;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Doctor
{
    public class DoctorHomeViewModel : BaseViewModel
    {
        public ObservableCollection<AccountDto> Drivers { get; set; }
        public Command<AccountDto> ShowReviewPopupCommand { get; }
        public DateTime CurrentDate { get; }

        public DoctorHomeViewModel()
        {
            CurrentDate = DateTime.Now;

            Drivers = new ObservableCollection<AccountDto>
            {
                new AccountDto { FirstName = "John", LastName = "Doe" },
                new AccountDto { FirstName = "Jane", LastName = "Smith" }
            };

            ShowReviewPopupCommand = new Command<AccountDto>(async (driver) => await ShowReviewPopup(driver));
        }

        private async Task ShowReviewPopup(AccountDto driver)
        {
            var reviewPopup = new DoctorReviewPopup
            {
                BindingContext = new DoctorReviewViewModel(driver)
            };
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(reviewPopup);
        }
    }
}
