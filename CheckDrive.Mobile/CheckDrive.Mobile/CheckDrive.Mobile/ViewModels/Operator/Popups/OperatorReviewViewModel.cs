using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Operator;
using CheckDrive.Mobile.Stores.Account;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Operator.Popups
{
    public class OperatorReviewViewModel : BaseViewModel
    {
        private readonly TaskCompletionSource<OperatorReviewRequest> _completionSource;
        private readonly IAccountStore _accountStore;
        private readonly CheckPointDto _checkPoint;
        private readonly CarDto _car;

        public List<OilMark> OilMarks { get; }

        private OilMark _selectedOilMark;
        public OilMark SelectedOilMark
        {
            get => _selectedOilMark;
            set => SetProperty(ref _selectedOilMark, value);
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        private string _oilMarkErrorMessage;
        public string OilMarkErrorMessage
        {
            get => _oilMarkErrorMessage;
            set => SetProperty(ref _oilMarkErrorMessage, value);
        }

        private string _totalOilAmountErrorMessage;
        public string TotalOilAmountErrorMessage
        {
            get => _totalOilAmountErrorMessage;
            set => SetProperty(ref _totalOilAmountErrorMessage, value);
        }

        private int _initialOilAmount;
        public int InitialOilAmount
        {
            get => _initialOilAmount;
            set
            {
                if (value < 0)
                {
                    return;
                }

                SetProperty(ref _initialOilAmount, value);

                CheckTotalOilAmount();
            }
        }

        private int _oilRefillAmount;
        public int OilRefillAmount
        {
            get => _oilRefillAmount;
            set
            {
                if (value < 0)
                {
                    return;
                }

                SetProperty(ref _oilRefillAmount, value);

                CheckTotalOilAmount();
            }
        }

        public string DriverName { get; }
        public string Car { get; }

        public Command ApproveCommand { get; }
        public Command CancelCommand { get; }

        public OperatorReviewViewModel(CheckPointDto checkPoint, List<OilMark> oilMarks, TaskCompletionSource<OperatorReviewRequest> completionSource)
        {
            _completionSource = completionSource;
            _accountStore = DependencyService.Get<IAccountStore>();
            _checkPoint = checkPoint;
            _car = checkPoint.Car;

            DriverName = checkPoint.DriverName;
            Car = checkPoint.Car.ToString();
            OilMarks = oilMarks;
            InitialOilAmount = (int)_car.RemainingFuel;
            SelectedOilMark = OilMarks.Find(x => x.Id == _car.OilMarkId) ?? OilMarks.FirstOrDefault();

            ApproveCommand = new Command(async () => await OnApproveAsync(), CanApprove);
            CancelCommand = new Command(async () => await OnCancelAsync());
        }

        private async Task OnApproveAsync()
        {
            var operatorId = await _accountStore.GetUserIdAsync();
            var review = new OperatorReviewRequest(
                checkPointId: _checkPoint.Id,
                operatorId: operatorId,
                oilMarkId: SelectedOilMark.Id,
                notes: Notes,
                initialOilAmount: InitialOilAmount,
                oilRefillAmount: OilRefillAmount);

            await PopupNavigation.Instance.PopAsync();

            _completionSource.SetResult(review);
        }

        private async Task OnCancelAsync()
        {
            await PopupNavigation.Instance.PopAsync();

            _completionSource.SetResult(null);
        }

        private bool CanApprove() => !IsTotalAmountExceedsCapacity();

        private void CheckTotalOilAmount()
        {
            if (IsTotalAmountExceedsCapacity())
            {
                TotalOilAmountErrorMessage = $"Avtomobilning yoqilg'i sig'imi {_car.FuelCapacity} litr";
            }
            else
            {
                TotalOilAmountErrorMessage = string.Empty;
            }
        }

        private bool IsTotalAmountExceedsCapacity()
            => _initialOilAmount + OilRefillAmount > _car.FuelCapacity;
    }
}
