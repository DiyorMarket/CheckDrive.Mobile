using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Mechanic.Acceptance;
using CheckDrive.Mobile.Stores.Account;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Mechanic.Popups
{
    public class MechanicAcceptanceReviewViewModel : BaseViewModel
    {
        private readonly IAccountStore _accountStore;
        private readonly TaskCompletionSource<MechanicAcceptanceRequest> _completionSource;
        private readonly CheckPointDto _checkPointDto;
        private int _minMileage;

        private int _finalMileage;
        public int FinalMileage
        {
            get => _finalMileage;
            set
            {
                SetProperty(ref _finalMileage, value);

                if (IsFinalMileageValid())
                {
                    FinalMileageErrorMessage = string.Empty;
                }
                else
                {
                    FinalMileageErrorMessage = $"Masofa ({_minMileage} km)dan katta bo'lishi kerak";
                }
            }
        }

        private bool _isCarInGoodCondition = true;
        public bool IsCarInGoodCondition
        {
            get => _isCarInGoodCondition;
            set
            {
                SetProperty(ref _isCarInGoodCondition, value);

                if (_isCarInGoodCondition)
                {
                    NotesErrorMessage = string.Empty;
                    SwitchText = "Avtomobil soz holatda";
                }
                else
                {
                    NotesErrorMessage = "Sababni ko'rsatish majburiy";
                    SwitchText = "Avtomobil nosoz holatda";
                }
            }
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        private string _notesErrorMessage;
        public string NotesErrorMessage
        {
            get => _notesErrorMessage;
            set => SetProperty(ref _notesErrorMessage, value);
        }

        private string _finalMileageErrorMessage;
        public string FinalMileageErrorMessage
        {
            get => _finalMileageErrorMessage;
            set => SetProperty(ref _finalMileageErrorMessage, value);
        }

        private string _switchText;
        public string SwitchText
        {
            get => _switchText;
            set => SetProperty(ref _switchText, value);
        }

        public string DriverName { get; set; }
        public string Car { get; set; }

        public Command ApproveCommand { get; }
        public Command CancelCommand { get; }

        public MechanicAcceptanceReviewViewModel(CheckPointDto checkPoint, TaskCompletionSource<MechanicAcceptanceRequest> completionSource)
        {
            _accountStore = DependencyService.Get<IAccountStore>();
            _checkPointDto = checkPoint;
            _completionSource = completionSource;

            ApproveCommand = new Command(async () => await OnApproveAsync(), CanApprove);
            CancelCommand = new Command(async () => await OnCancelAsync());

            SetupInitialValues(checkPoint);
        }

        private void SetupInitialValues(CheckPointDto checkPoint)
        {
            DriverName = checkPoint.DriverName;
            Car = checkPoint.Car.ToString();
            FinalMileage = checkPoint.MechanicHandover.InitialMileage;
            _minMileage = checkPoint.MechanicHandover.InitialMileage;
        }

        private async Task OnApproveAsync()
        {
            var reviewerId = await _accountStore.GetUserIdAsync();
            var review = new MechanicAcceptanceRequest(
                checkPointId: _checkPointDto.Id,
                mechanicId: reviewerId,
                finalMileage: FinalMileage,
                isCarInGoodCondition: IsCarInGoodCondition,
                notes: Notes);

            await PopupNavigation.Instance.PopAsync();

            _completionSource.SetResult(review);
        }

        private async Task OnCancelAsync()
        {
            await PopupNavigation.Instance.PopAsync();

            _completionSource.SetResult(null);
        }

        private bool CanApprove()
            => (_isCarInGoodCondition || !string.IsNullOrWhiteSpace(Notes)) && IsFinalMileageValid();

        private bool IsFinalMileageValid() => _finalMileage >= _minMileage;
    }
}
