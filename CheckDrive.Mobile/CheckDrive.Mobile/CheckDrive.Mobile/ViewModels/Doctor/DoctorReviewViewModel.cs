using CheckDrive.Mobile.Models;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Doctor
{
    public class DoctorReviewViewModel : BaseViewModel
    {
        public string FullName { get; }

        private bool _isHealthy = false;
        public bool IsHealthy
        {
            get => _isHealthy;
            set => SetProperty(ref _isHealthy, value);
        }
        public string Comments { get; set; }

        public Command ApproveCommand { get; }
        public Command CancelCommand { get; }

        private readonly AccountDto _driver;

        public DoctorReviewViewModel(AccountDto driver)
        {
            _driver = driver;
            FullName = $"{driver.FirstName} {driver.LastName}";
            ApproveCommand = new Command(OnApprove);
            CancelCommand = new Command(OnCancel);
        }

        private async void OnApprove()
        {
            // Save the review logic here
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        private async void OnCancel()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}
