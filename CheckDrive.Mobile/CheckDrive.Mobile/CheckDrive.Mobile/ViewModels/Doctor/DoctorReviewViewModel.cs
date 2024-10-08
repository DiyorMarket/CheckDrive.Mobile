using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Stores.Account;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Doctor
{
    public class DoctorReviewViewModel : BaseViewModel
    {
        private readonly IAccountStore _accountStore;
        private readonly TaskCompletionSource<DoctorReview> _completionSource;

        public string FullName { get; }
        public int DriverId { get; set; }

        private bool _isHealthy = false;
        public bool IsHealthy
        {
            get => _isHealthy;
            set => SetProperty(ref _isHealthy, value);
        }

        public string Notes { get; set; }

        public Command ApproveCommand { get; }
        public Command CancelCommand { get; }

        public DoctorReviewViewModel(DriverDto driver, TaskCompletionSource<DoctorReview> completionSource)
        {
            _accountStore = DependencyService.Get<IAccountStore>();
            _completionSource = completionSource;

            FullName = driver.FullName;
            DriverId = driver.Id;

            ApproveCommand = new Command(OnApprove);
            CancelCommand = new Command(OnCancel);
        }

        private async void OnApprove()
        {
            var reviewerId = await _accountStore.GetEmployeeIdAsync();
            var review = new DoctorReview(reviewerId, Notes, IsHealthy, DriverId);

            _completionSource.SetResult(review);
        }

        private async void OnCancel()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}
