using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Stores.Account;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Dispatcher.Popups
{
    public class DispatcherReviewViewModel : BaseViewModel
    {
        private readonly TaskCompletionSource<DispatcherReview> _completionSource;
        private readonly IAccountStore _accountStore;
        private readonly CheckPointDto _checkPoint;

        public string DriverName { get; }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public decimal FuelConsumptionAdjustment { get; set; }
        public int FinalMileageAdjustment { get; set; }

        public Command ApproveCommand { get; }
        public Command RejectCommand { get; }

        public DispatcherReviewViewModel(CheckPointDto checkPoint, TaskCompletionSource<DispatcherReview> completionSource)
        {
            _completionSource = completionSource;
            _accountStore = DependencyService.Get<IAccountStore>();
            _checkPoint = checkPoint;

            DriverName = checkPoint.DriverName;
            FuelConsumptionAdjustment = checkPoint.MechanicAcceptance.RemainingFuelAmount;
            FinalMileageAdjustment = checkPoint.MechanicAcceptance.FinalMileage;

            ApproveCommand = new Command(async () => await OnApprove());
            RejectCommand = new Command(async () => await OnReject());
        }

        private async Task OnApprove()
        {
            var reviewerId = await _accountStore.GetUserIdAsync();

            var review = new DispatcherReview(
                _checkPoint.Id,
                reviewerId,
                Notes,
                true,
                null,
                null);

            if (FuelConsumptionAdjustment != _checkPoint.MechanicAcceptance.RemainingFuelAmount)
            {
                review.FuelConsumptionAdjustment = FuelConsumptionAdjustment;
            }

            if (FinalMileageAdjustment != _checkPoint.MechanicAcceptance.FinalMileage)
            {
                review.DistanceTravelledAdjustment = FinalMileageAdjustment;
            }

            _completionSource.SetResult(review);
        }

        private async Task OnReject()
        {
            var reviewerId = await _accountStore.GetUserIdAsync();
            var review = new DispatcherReview(
                _checkPoint.Id,
                reviewerId,
                Notes,
                false,
                null,
                null);

            _completionSource.SetResult(review);
        }
    }
}
