using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Driver.Popups
{
    public abstract class BaseReviewConfirmationViewModel : BaseViewModel
    {
        protected readonly TaskCompletionSource<ReviewConfirmationRequest> _completionSource;
        protected readonly int _checkPointId;
        protected readonly ReviewType _reviewType;

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public Command RejectCommand { get; }
        public Command AcceptCommand { get; }

        protected BaseReviewConfirmationViewModel(
            TaskCompletionSource<ReviewConfirmationRequest> completionSource,
            int checkPointId,
            ReviewType reviewType)
        {
            _completionSource = completionSource;
            _checkPointId = checkPointId;
            _reviewType = reviewType;

            RejectCommand = new Command(async () => await OnRejectAsync());
            AcceptCommand = new Command(async () => await OnAcceptAsync());
        }

        private async Task OnRejectAsync()
        {
            var result = new ReviewConfirmationRequest(
                checkPointId: _checkPointId,
                reviewType: _reviewType,
                isAcceptedByDriver: false,
                notes: Notes);

            await PopupNavigation.Instance.PopAsync();

            _completionSource.SetResult(result);
        }

        private async Task OnAcceptAsync()
        {
            var result = new ReviewConfirmationRequest(
                checkPointId: _checkPointId,
                reviewType: _reviewType,
                isAcceptedByDriver: true,
                notes: Notes);

            await PopupNavigation.Instance.PopAsync();

            _completionSource.SetResult(result);
        }
    }
}
