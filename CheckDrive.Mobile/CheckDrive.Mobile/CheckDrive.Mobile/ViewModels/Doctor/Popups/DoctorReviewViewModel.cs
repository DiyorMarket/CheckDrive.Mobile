using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Stores.Account;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Doctor.Popups
{
    public class DoctorReviewViewModel : BaseViewModel
    {
        private readonly IAccountStore _accountStore;
        private readonly TaskCompletionSource<DoctorReviewRequest> _completionSource;

        public string FullName { get; }
        public int DriverId { get; set; }

        private bool _isHealthy;
        public bool IsHealthy
        {
            get => _isHealthy;
            set
            {
                SetProperty(ref _isHealthy, value);

                if (_isHealthy)
                {
                    NotesErrorMessage = string.Empty;
                    SwitchText = "Sog'lom";
                }
                else
                {
                    NotesErrorMessage = "Sababni ko'rsatish majburiy";
                    SwitchText = "Sog'lom emas";
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

        private string _switchText;
        public string SwitchText
        {
            get => _switchText;
            set => SetProperty(ref _switchText, value);
        }

        public Command ApproveCommand { get; }
        public Command CancelCommand { get; }

        public DoctorReviewViewModel(DriverDto driver, TaskCompletionSource<DoctorReviewRequest> completionSource)
        {
            _accountStore = DependencyService.Get<IAccountStore>();
            _completionSource = completionSource;

            FullName = driver.FullName;
            DriverId = driver.Id;
            IsHealthy = true;

            ApproveCommand = new Command(async () => await OnApproveAsync(), CanApprove);
            CancelCommand = new Command(async () => await OnCancelAsync());
        }

        private async Task OnApproveAsync()
        {
            try
            {
                var reviewerId = await _accountStore.GetUserIdAsync();
                var review = new DoctorReviewRequest(
                    driverId: DriverId,
                    doctorId: reviewerId,
                    isHealthy: IsHealthy,
                    notes: Notes);

                await PopupNavigation.Instance.PopAsync();

                _completionSource.SetResult(review);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task OnCancelAsync()
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();

                _completionSource.SetResult(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private bool CanApprove()
            => IsHealthy || !string.IsNullOrWhiteSpace(Notes);
    }
}
