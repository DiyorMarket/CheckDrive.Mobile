using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Stores.Account;
using System.Threading.Tasks;
using Xamarin.Forms;
using CheckDrive.Mobile.Models.Dispatcher;
using Rg.Plugins.Popup.Services;

namespace CheckDrive.Mobile.ViewModels.Dispatcher.Popups
{
    public class DispatcherReviewViewModel : BaseViewModel
    {
        private readonly TaskCompletionSource<DispatcherReviewRequest> _completionSource;
        private readonly IAccountStore _accountStore;
        private readonly CheckPointDto _checkPoint;
        private readonly CarDto _car;
        private readonly int _minFinalMileage;

        private int _finalMileage;
        public int FinalMileage
        {
            get => _finalMileage;
            set
            {
                SetProperty(ref _finalMileage, value);

                ValidateFinalMileage();
                UpdateFuelConsumptionAmount();
            }
        }

        private decimal _fuelConsumptionAmount;
        public decimal FuelConsumptionAmount
        {
            get => _fuelConsumptionAmount;
            set => SetProperty(ref _fuelConsumptionAmount, value);
        }

        private decimal _remainingFuelAmount;
        public decimal RemainingFuelAmount
        {
            get => _remainingFuelAmount;
            set => SetProperty(ref _remainingFuelAmount, value);
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        private string _finalMileageErrorMessage;
        public string FinalMileageErrorMessage
        {
            get => _finalMileageErrorMessage;
            set => SetProperty(ref _finalMileageErrorMessage, value);
        }

        public string DriverName { get; }
        public string Car { get; }
        public int InitialMileageByMechanic { get; }
        public int FinalMileageByMechanic { get; }

        public Command ApproveCommand { get; }
        public Command CancelCommand { get; }

        public DispatcherReviewViewModel(CheckPointDto checkPoint, TaskCompletionSource<DispatcherReviewRequest> completionSource)
        {
            _completionSource = completionSource;
            _accountStore = DependencyService.Get<IAccountStore>();
            _checkPoint = checkPoint;
            _car = checkPoint.Car;
            _minFinalMileage = checkPoint.MechanicHandover.InitialMileage;

            DriverName = checkPoint.DriverName;
            Car = checkPoint.Car.ToString();
            InitialMileageByMechanic = checkPoint.MechanicHandover.InitialMileage;
            FinalMileageByMechanic = checkPoint.MechanicAcceptance.FinalMileage;
            FinalMileage = checkPoint.MechanicAcceptance.FinalMileage;

            ApproveCommand = new Command(async () => await OnApproveAsync(), CanApprove);
            CancelCommand = new Command(async () => await OnRejectAsync());
        }

        private async Task OnApproveAsync()
        {
            var dispatcherId = await _accountStore.GetUserIdAsync();
            var result = new DispatcherReviewRequest(
                checkPointId: _checkPoint.Id,
                dispatcherId: dispatcherId,
                notes: Notes,
                finalMileage: FinalMileage,
                fuelConsumptionAmount: FuelConsumptionAmount,
                remainingFuelAmount: RemainingFuelAmount);

            await PopupNavigation.Instance.PopAsync();

            _completionSource.SetResult(result);
        }

        private async Task OnRejectAsync()
        {
            await PopupNavigation.Instance.PopAsync();

            _completionSource.SetResult(null);
        }

        private bool CanApprove() => IsFinalMileageValid();

        private void ValidateFinalMileage()
        {
            if (!IsFinalMileageValid())
            {
                FinalMileageErrorMessage = $"Masofa ({_minFinalMileage} km)dan ko'p bo'lishi kerak";
            }
            else
            {
                FinalMileageErrorMessage = string.Empty;
            }
        }

        private bool IsFinalMileageValid() => _finalMileage >= _minFinalMileage;

        private void UpdateFuelConsumptionAmount()
        {
            var distance = FinalMileage - _checkPoint.MechanicHandover.InitialMileage;

            if (distance > 0)
            {
                FuelConsumptionAmount = (_car.AverageFuelConsumption * distance) / 100;
            }
            else
            {
                FuelConsumptionAmount = 0;
            }
        }
    }
}
