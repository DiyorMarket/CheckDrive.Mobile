using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Stores.Account;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Mechanic.Popups
{
    public class MechanicHandoverReviewViewModel : BaseViewModel
    {
        private readonly CheckPointDto _checkPoint;
        private readonly IAccountStore _accountStore;
        private readonly TaskCompletionSource<MechanicHandoverReview> _completionSource;

        public DriverDto Driver { get; }
        public List<CarDto> Cars { get; }

        private CarDto _selectedCar;
        public CarDto SelectedCar
        {
            get => _selectedCar;
            set
            {
                SetProperty(ref _selectedCar, value);
                InitialMileage = value.Mileage;
            }
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        private bool _isApproved;
        public bool IsApproved
        {
            get => _isApproved;
            set => SetProperty(ref _isApproved, value);
        }

        private int _initialMileage;
        public int InitialMileage
        {
            get => _initialMileage;
            set => SetProperty(ref _initialMileage, value);
        }

        private string _driverName;
        public string DriverName
        {
            get => _driverName;
            private set => SetProperty(ref _driverName, value);
        }

        public Command ApproveCommand { get; }
        public Command CancelCommand { get; }

        public MechanicHandoverReviewViewModel(
            CheckPointDto checkPoint,
            List<CarDto> cars,
            TaskCompletionSource<MechanicHandoverReview> taskCompletionSource)
        {
            _checkPoint = checkPoint;
            _accountStore = DependencyService.Get<IAccountStore>();

            _completionSource = taskCompletionSource;
            DriverName = checkPoint.DriverName;
            Cars = new List<CarDto>(cars);
            SelectedCar = cars[0];
            InitialMileage = SelectedCar.Mileage;
            IsApproved = false;

            ApproveCommand = new Command(async () => await OnApprove());
            CancelCommand = new Command(async () => await OnCancel());
        }

        private async Task OnApprove()
        {
            var reviewerId = await _accountStore.GetUserIdAsync();
            var review = new MechanicHandoverReview(reviewerId, Notes, IsApproved, SelectedCar.Id, InitialMileage, _checkPoint.Id, SelectedCar);

            _completionSource.SetResult(review);
        }

        private async Task OnCancel()
        {
            _completionSource.SetResult(null);
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}
