using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Review;
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
        public ObservableCollection<DriverDto> Drivers { get; set; }
        public Command<DriverDto> ShowReviewPopupCommand { get; }
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
            Drivers = new ObservableCollection<DriverDto>
            {
                new DriverDto { FullName = "John de Brueny" },
                new DriverDto { FullName = "Jane Smith"}
            };

            ShowReviewPopupCommand = new Command<DriverDto>(async (driver) => await ShowReviewPopup(driver));
        }

        private async Task ShowReviewPopup(DriverDto driver)
        {
            var completionSource = new TaskCompletionSource<OperatorReview>();
            var reviewPopup = new OperatorReviewPopup
            {
                BindingContext = new OperatorReviewViewModel(driver, OilMarks)
            };
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(reviewPopup);

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();

            //await SendReviewAsync(result, driver.FullName);
        }
    }
}
