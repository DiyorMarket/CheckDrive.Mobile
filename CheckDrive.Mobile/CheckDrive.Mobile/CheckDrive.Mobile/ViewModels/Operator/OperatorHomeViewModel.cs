using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Views.Operator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Operator
{
    public class OperatorHomeViewModel : BaseViewModel
    {
        public List<OilMark> OilMarks { get; set; }
        public ObservableCollection<AccountDto> Drivers { get; set; }
        public Command<AccountDto> ShowReviewPopupCommand { get; }
        public DateTime CurrentDate { get; }

        public OperatorHomeViewModel()
        {
            CurrentDate = DateTime.Now;

            OilMarks = new List<OilMark>
            {
                new OilMark { Id = 1, Name = "80" },
                new OilMark { Id = 2, Name = "85" },
                new OilMark { Id = 3, Name = "90" },
                new OilMark { Id = 4, Name = "92" },
            };
            Drivers = new ObservableCollection<AccountDto>
            {
                new AccountDto { FirstName = "John", LastName = "Doe" },
                new AccountDto { FirstName = "Jane", LastName = "Smith" }
            };

            ShowReviewPopupCommand = new Command<AccountDto>(async (driver) => await ShowReviewPopup(driver));
        }

        private async Task ShowReviewPopup(AccountDto driver)
        {
            var reviewPopup = new OperatorReviewPopup
            {
                BindingContext = new OperatorReviewViewModel(driver, OilMarks)
            };
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(reviewPopup);
        }
    }
}
