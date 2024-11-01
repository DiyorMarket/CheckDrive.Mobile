using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Stores.Account;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Operator
{
    public class OperatorReviewViewModel : BaseViewModel
    {
        private readonly TaskCompletionSource<OperatorReview> _completionSource;
        private readonly IAccountStore _accountStore;
        private readonly CheckPointDto _checkPoint;

        public List<OilMark> OilMarks { get; }
        public string DriverName { get; }

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

        public int InitialOilAmount { get; set; }
        public int OilRefillAmount { get; set; }

        public Command ApproveCommand { get; }
        public Command RejectCommand { get; }

        public OperatorReviewViewModel(CheckPointDto checkPoint, List<OilMark> oilMarks, TaskCompletionSource<OperatorReview> completionSource)
        {
            _completionSource = completionSource;
            _accountStore = DependencyService.Get<IAccountStore>();
            _checkPoint = checkPoint;

            DriverName = checkPoint.DriverName;
            OilMarks = oilMarks;
            InitialOilAmount = (int)checkPoint.Car.RemainingFuel;
            SelectedOilMark = OilMarks[0];

            ApproveCommand = new Command(async () => await OnApprove());
            RejectCommand = new Command(async () => await OnReject());
        }

        private async Task OnApprove()
        {
            var reviewerId = await _accountStore.GetUserIdAsync();
            var review = new OperatorReview(
                reviewerId,
                Notes,
                true,
                _checkPoint.Id,
                SelectedOilMark.Id,
                SelectedOilMark.Name,
                InitialOilAmount,
                OilRefillAmount);

            _completionSource.SetResult(review);
        }

        private async Task OnReject()
        {
            var reviewerId = await _accountStore.GetUserIdAsync();
            var review = new OperatorReview(reviewerId, Notes, false, _checkPoint.Id, 0, null, 0, 0);

            _completionSource.SetResult(review);
        }
    }
}
