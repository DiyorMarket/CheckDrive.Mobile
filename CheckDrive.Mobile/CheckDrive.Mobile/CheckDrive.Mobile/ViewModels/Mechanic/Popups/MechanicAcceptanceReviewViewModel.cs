using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Stores.Account;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Mechanic.Popups
{
    public class MechanicAcceptanceReviewViewModel : BaseViewModel
    {
        private readonly IAccountStore _accountStore;
        private readonly TaskCompletionSource<MechanicAcceptanceReview> _completionSource;
        private readonly CheckPointDto _checkPointDto;

        private int _finalMileage;
        public int FinalMileage
        {
            get => _finalMileage;
            set => SetProperty(ref _finalMileage, value);
        }

        private decimal _remainingFuelAmount;
        public decimal RemainingFuelAmount // Calculate on mileage change based on the car's average consumption, if exceeds then set to 0, allow users enter manually as well
        {
            get => _remainingFuelAmount;
            set => SetProperty(ref _remainingFuelAmount, value);
        }

        private bool _isApproved;
        public bool IsApproved
        {
            get => _isApproved;
            set => SetProperty(ref _isApproved, value);
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public string DriverName { get; set; }
        public string Car { get; set; }

        public Command ApproveCommand { get; }
        public Command CancelCommand { get; }

        public MechanicAcceptanceReviewViewModel(CheckPointDto checkPoint, TaskCompletionSource<MechanicAcceptanceReview> completionSource)
        {
            _accountStore = DependencyService.Get<IAccountStore>();
            _checkPointDto = checkPoint;
            _completionSource = completionSource;

            DriverName = checkPoint.DriverName;
            Car = checkPoint.Car.ToString();
            FinalMileage = checkPoint.Car.Mileage;

            ApproveCommand = new Command(async () => await OnApproveAsync());
            CancelCommand = new Command(async () => await OnCancelAsync());
        }

        private async Task OnApproveAsync()
        {
            var reviewerId = await _accountStore.GetUserIdAsync();
            var review = new MechanicAcceptanceReview(reviewerId, Notes, IsApproved, FinalMileage, _checkPointDto.Id, RemainingFuelAmount);

            _completionSource.SetResult(review);
        }

        private async Task OnCancelAsync()
        {
            _completionSource.SetResult(null);
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}
