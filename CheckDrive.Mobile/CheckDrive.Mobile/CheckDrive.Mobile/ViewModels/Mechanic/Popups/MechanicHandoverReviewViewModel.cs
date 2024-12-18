using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Mechanic.Handover;
using CheckDrive.Mobile.Stores.Account;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Mechanic.Popups
{
    public class MechanicHandoverReviewViewModel : BaseViewModel
    {
        private readonly CheckPointDto _checkPoint;
        private readonly IAccountStore _accountStore;
        private readonly TaskCompletionSource<MechanicHandoverRequest> _completionSource;
        private int _minMileage;

        public DriverDto Driver { get; }
        public List<CarDto> Cars { get; }

        private CarDto _selectedCar;
        public CarDto SelectedCar
        {
            get => _selectedCar;
            set
            {
                SetProperty(ref _selectedCar, value);
                _minMileage = value?.Mileage ?? 0;
                InitialMileage = value?.Mileage ?? 0;

                if (value is null)
                {
                    CarErrorMessage = "Avtomobilni tanlash majburiy";
                }
            }
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        private int _initialMileage;
        public int InitialMileage
        {
            get => _initialMileage;
            set
            {
                SetProperty(ref _initialMileage, value);

                if (IsInitialMileageValid())
                {
                    InitialMileageErrorMessage = string.Empty;
                }
                else
                {
                    InitialMileageErrorMessage = $"Masofa ({_minMileage} km)dan katta bo'lishi kerak";
                }
            }
        }

        private string _driverName;
        public string DriverName
        {
            get => _driverName;
            private set => SetProperty(ref _driverName, value);
        }

        private string _initialMileageErrorMessage;
        public string InitialMileageErrorMessage
        {
            get => _initialMileageErrorMessage;
            set => SetProperty(ref _initialMileageErrorMessage, value);
        }

        private string _carErrorMessage;
        public string CarErrorMessage
        {
            get => _carErrorMessage;
            set => SetProperty(ref _carErrorMessage, value);
        }

        public Command ApproveCommand { get; }
        public Command CancelCommand { get; }

        public MechanicHandoverReviewViewModel(
            CheckPointDto checkPoint,
            List<CarDto> cars,
            TaskCompletionSource<MechanicHandoverRequest> taskCompletionSource)
        {
            _checkPoint = checkPoint;
            _accountStore = DependencyService.Get<IAccountStore>();
            _completionSource = taskCompletionSource;

            ApproveCommand = new Command(async () => await OnApprove(), CanApprove);
            CancelCommand = new Command(async () => await OnCancel());

            Cars = new List<CarDto>(cars);

            SetupInitialValues(checkPoint, cars);
        }

        private void SetupInitialValues(CheckPointDto checkPoint, List<CarDto> cars)
        {
            var driver = checkPoint.Driver;

            DriverName = driver.FullName;
            var assignedCar = driver.AssignedCarId.HasValue
                ? cars.Find(x => x != null && x.Id == driver.AssignedCarId.Value)
                : cars.FirstOrDefault();
            SelectedCar = assignedCar is null
                ? cars.FirstOrDefault()
                : assignedCar;
            InitialMileage = SelectedCar?.Mileage ?? 0;
            _minMileage = InitialMileage;
        }

        private async Task OnApprove()
        {
            var reviewerId = await _accountStore.GetUserIdAsync();
            var review = new MechanicHandoverRequest(
                checkPointId: _checkPoint.Id,
                mechanicId: reviewerId,
                carId: SelectedCar.Id,
                initialMileage: InitialMileage,
                notes: Notes);

            _completionSource.SetResult(review);
        }

        private async Task OnCancel()
        {
            _completionSource.SetResult(null);
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        private bool CanApprove()
            => IsInitialMileageValid();

        private bool IsInitialMileageValid() => _initialMileage >= _minMileage;
    }
}
