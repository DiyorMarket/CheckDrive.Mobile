using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Stores.Account;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Operator
{
    public class OperatorReviewViewModel : BaseViewModel
    {
        private readonly IAccountStore _accountStore;
        private readonly TaskCompletionSource<OperatorReview> _completionSource;

        private readonly int _driverId;
        public string FullName { get; }
        public List<OilMark> OilMarks { get; }
        public OilMark SelectedOilMark { get; set; }
        public string InitialOilAmount { get; set; }
        public string OilRefilAmount { get; set; }
        public string Comments { get; set; }

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        public OperatorReviewViewModel(DriverDto driver, List<OilMark> oilMarks, TaskCompletionSource<OperatorReview> completionSource)
        {
            _accountStore = DependencyService.Get<IAccountStore>();
            _driverId = driver.Id;
            FullName = driver.FullName;
            OilMarks = oilMarks ?? new List<OilMark>();
            _completionSource = completionSource;

            ApproveCommand = new Command(OnApprove);
            RejectCommand = new Command(OnReject);
        }

        private async void OnApprove()
        {
            int reviewerId = await _accountStore.GetEmployeeIdAsync();

            // Assuming TryParse is used for validation
            if (!double.TryParse(InitialOilAmount, out _) || !double.TryParse(OilRefilAmount, out _))
            {
                // Handle invalid input here (e.g., show an alert to the user)
                return;
            }

            var review = new OperatorReview(
                reviewerId,
                Comments,
                isApprovedByReviewer: true,
                _driverId,
                FullName,
                SelectedOilMark,
                InitialOilAmount,
                OilRefilAmount,
                Comments
            );

            _completionSource.SetResult(review);
            ClosePopup();
        }

        private void OnReject()
        {
            _completionSource.SetResult(null); // Or create a rejected review if needed
            ClosePopup();
        }

        private async void ClosePopup()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}
