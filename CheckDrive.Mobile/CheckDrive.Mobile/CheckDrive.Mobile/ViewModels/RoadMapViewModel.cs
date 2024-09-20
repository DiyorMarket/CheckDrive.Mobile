using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.ApiContracts.Driver;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Stores.DispatcherReviewDataStore;
using CheckDrive.Mobile.Stores.DoctorReviews;
using CheckDrive.Mobile.Stores.MechanicAcceptances;
using CheckDrive.Mobile.Stores.MechanicHandovers;
using CheckDrive.Mobile.Stores.OperatorReviews;
using CheckDrive.Mobile.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class RoadMapViewModel : BaseViewModel
    {
        private readonly IDoctorReviewDataStore _doctorReviewDatastore;
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptanceDataStore;
        private readonly IMechanicHandoverDataStore _mechanicHandoverDataStore;
        private readonly IOperatorReviewDataStore _operatorReviewDataStore;
        private readonly IDispatcherReviewDataStore _dispatcherReviewDataStore;

        #region Popup Properties 
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand RejectButtonCommand { get; set; }

        private bool _isAcceptButtonEnabled = true;
        public bool IsAcceptButtonEnabled
        {
            get => _isAcceptButtonEnabled;
            set
            {
                _isAcceptButtonEnabled = value;
                OnPropertyChanged(nameof(IsAcceptButtonEnabled));
            }
        }

        private bool _isRejectButtonEnabled = true;
        public bool IsRejectButtonEnabled
        {
            get => _isRejectButtonEnabled;
            set
            {
                _isRejectButtonEnabled = value;
                OnPropertyChanged(nameof(IsRejectButtonEnabled));
            }
        }

        private Color _acceptButtonColor = Color.FromHex("#2A7BEF");
        public Color AcceptButtonColor
        {
            get => _acceptButtonColor;
            set
            {
                _acceptButtonColor = value;
                OnPropertyChanged(nameof(AcceptButtonColor));
            }
        }

        private Color _rejectButtonColor = Color.FromHex("#85929E");
        public Color RejectButtonColor
        {
            get => _rejectButtonColor;
            set
            {
                _rejectButtonColor = value;
                OnPropertyChanged(nameof(RejectButtonColor));
            }
        }
        #endregion

        public DateTime StartDateForProgressBar { get; set; }
        public DateTime TodayDateForProgressBar { get; set; }
        public DateTime EndDateForProgressBar { get; set; }
        private DriverDto _driver;
        private readonly SignalRService _signalRService;

        private StatusForDto _doctorStatus;
        public StatusForDto DoctorStatusCheck
        {
            get { return _doctorStatus; }
            set
            {
                SetProperty(ref _doctorStatus, value);
                OnPropertyChanged(nameof(DoctorStatusCheck));
            }
        }

        private StatusForDto _mechanicAcceptanceStatus;
        public StatusForDto MechanicAcceptanceStatusCheck
        {
            get { return _mechanicAcceptanceStatus; }
            set
            {
                SetProperty(ref _mechanicAcceptanceStatus, value);
                OnPropertyChanged(nameof(MechanicAcceptanceStatusCheck));
            }
        }

        private StatusForDto _operatorStatus;
        public StatusForDto OperatorStatusCheck
        {
            get { return _operatorStatus; }
            set
            {
                SetProperty(ref _operatorStatus, value);
                OnPropertyChanged(nameof(OperatorStatusCheck));
            }
        }

        private StatusForDto _mechanicHandoverStatus;
        public StatusForDto MechanicHandoverStatusCheck
        {
            get { return _mechanicHandoverStatus; }
            set
            {
                SetProperty(ref _mechanicHandoverStatus, value);
                OnPropertyChanged(nameof(MechanicHandoverStatusCheck));
            }
        }

        private double _oilPresentValue;
        public double OilPresentValue
        {
            get => _oilPresentValue;
            set
            {
                if (_oilPresentValue != value)
                {
                    _oilPresentValue = value;
                    OnPropertyChanged(nameof(OilPresentValue));
                }
            }
        }

        private string _oilValueToString;
        public string OilValueToString
        {
            get => _oilValueToString;
            set
            {
                if (_oilValueToString != value)
                {
                    _oilValueToString = value;
                    OnPropertyChanged(nameof(OilValueToString));
                }
            }
        }
        private float oilPercent;
        public float OilPercent
        {
            get => oilPercent;
            set
            {
                if (oilPercent != value)
                {
                    oilPercent = value;
                    OnPropertyChanged(nameof(OilPercent));
                }
            }
        }

        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        private string _doctorCheckTime = "";
        public string DoctorCheckTime
        {
            get => _doctorCheckTime;
            set
            {
                if (_doctorCheckTime != value)
                {
                    _doctorCheckTime = value;
                    OnPropertyChanged(nameof(DoctorCheckTime));
                }
            }
        }

        private string _mechanicHandoverCheckTime = "";
        public string MechanicHandoverCheckTime
        {
            get => _mechanicHandoverCheckTime;
            set
            {
                if (_mechanicHandoverCheckTime != value)
                {
                    _mechanicHandoverCheckTime = value;
                    OnPropertyChanged(nameof(MechanicHandoverCheckTime));
                }
            }
        }

        private string _operatorCheckTime = "";
        public string OperatorCheckTime
        {
            get => _operatorCheckTime;
            set
            {
                if (_operatorCheckTime != value)
                {
                    _operatorCheckTime = value;
                    OnPropertyChanged(nameof(OperatorCheckTime));
                }
            }
        }

        private string _mechanicAcceptanceCheckTime = "";
        public string MechanicAcceptanceCheckTime
        {
            get => _mechanicAcceptanceCheckTime;
            set
            {
                if (_mechanicAcceptanceCheckTime != value)
                {
                    _mechanicAcceptanceCheckTime = value;
                    OnPropertyChanged(nameof(MechanicAcceptanceCheckTime));
                }
            }
        }

        public RoadMapViewModel(IDoctorReviewDataStore doctorReviewDataStore,
            IMechanicAcceptanceDataStore mechanicAcceptanceDataStore,
            IOperatorReviewDataStore operatorReviewDataStore,
            IMechanicHandoverDataStore mechanicHandoverDataStore,
            IDispatcherReviewDataStore dispatcherReviewDataStore)
        {
            _doctorReviewDatastore = doctorReviewDataStore;
            _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;
            _operatorReviewDataStore = operatorReviewDataStore;
            _mechanicHandoverDataStore = mechanicHandoverDataStore;
            _dispatcherReviewDataStore = dispatcherReviewDataStore;
            _driver = LocalStorage.GetAccount();
            _signalRService = new SignalRService();
            AcceptButtonCommand = new Command(async () => await AcceptButton());
            RejectButtonCommand = new Command(async () => await RejectButton());

            LoadViewPage();
        }

        public async void LoadViewPage()
        {
            IsBusy = true;
            await GetOilResult();
            await CheckDoctorStatusValue();
            await CheckNotification();
            IsBusy = false;
        }

        private async Task GetOilResult()
        {
            GetDateForProgressBar();
            try
            {
                var dispatcherReviews = await _dispatcherReviewDataStore.GetDispatcherReviewResponses(_driver.Id);
                var driverhistoryForOil = dispatcherReviews.Data.ToList();

                while (dispatcherReviews.HasNextPage)
                {
                    dispatcherReviews = await _dispatcherReviewDataStore.GetDispatcherReviewResponses(_driver.Id);

                    driverhistoryForOil.Concat(dispatcherReviews.Data);
                }

                foreach (var dispatcherReview in driverhistoryForOil)
                {
                    if (dispatcherReview.Date >= StartDateForProgressBar
                        && dispatcherReview.Date <= DateTime.Now)
                    {
                        _oilPresentValue += dispatcherReview.FuelSpended;
                    }
                }
                OilValueToString = $"{(int)_oilPresentValue} L";
                OilPercent = (float)(_oilPresentValue / 450);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"new exception : {ex.Message}");
                MessagingCenter.Send(this, "UnauthorizedAccessDetected");
                return;
            }
        }

        private void GetDateForProgressBar()
        {
            StartDateForProgressBar = DateTime.Now.Date.AddDays(-(DateTime.Now.Date.Day - 1));
            TodayDateForProgressBar = DateTime.Now.Date;
            EndDateForProgressBar = DateTime.Now.Date.AddMonths(+1).AddDays(-DateTime.Now.Date.Day);
        }

        private async Task CheckNotification()
        {
            await _signalRService.StartConnectionAsync();
        }


        #region Departments check status value methods

        private async Task CheckDoctorStatusValue()
        {
            var doctorReviewsResponse = await _doctorReviewDatastore.GetDoctorReviewsAsync(TodayDateForProgressBar);
            var doctorReviews = doctorReviewsResponse.Data;

            var doctorReview = doctorReviews.FirstOrDefault(x => x.DriverId == _driver.Id);

            if (doctorReview != null)
            {
                if (doctorReview.IsHealthy.Value)
                {
                    DoctorStatusCheck = StatusForDto.Completed;
                    ChangedDoctorCheckTime(doctorReview);
                    await CheckMechanicHandoverStatusValue();
                    return;
                }

                DoctorStatusCheck = StatusForDto.Rejected;
                MechanicHandoverStatusCheck = StatusForDto.Rejected;
                OperatorStatusCheck = StatusForDto.Rejected;
                MechanicAcceptanceStatusCheck = StatusForDto.Rejected;
                ChangedDoctorCheckTime(doctorReview);
            }
        }

        private async Task CheckMechanicHandoverStatusValue()
        {
            var mechanicHandovers = await _mechanicHandoverDataStore.GetMechanicHandoversAsync(TodayDateForProgressBar);
            var mechanicHandover = mechanicHandovers.Data.FirstOrDefault(x => x.DriverId == _driver.Id);

            if (mechanicHandover != null)
            {
                if (mechanicHandover.IsHanded.Value && mechanicHandover.Status == StatusForDto.Completed)
                {
                    MechanicHandoverStatusCheck = StatusForDto.Completed;
                    ChangedMechanicHandoverCheckTime(mechanicHandover);
                    await CheckOperatorStatusValue();
                    return;
                }
                else if(mechanicHandover.Status == StatusForDto.Pending)
                {
                    MechanicHandoverStatusCheck = StatusForDto.Pending;
                    return;
                }

                MechanicHandoverStatusCheck = StatusForDto.Rejected;
                OperatorStatusCheck = StatusForDto.Rejected;
                MechanicAcceptanceStatusCheck = StatusForDto.Rejected;
                ChangedMechanicHandoverCheckTime(mechanicHandover);
            }
        }

        private async Task CheckOperatorStatusValue()
        {
            var operatorReviewResponse = await _operatorReviewDataStore.GetOperatorReviewsAsync(TodayDateForProgressBar);
            var operatorReview = operatorReviewResponse.Data.FirstOrDefault(x => x.DriverId == _driver.Id);

            if (operatorReview != null)
            {
                if (operatorReview.IsGiven.Value && operatorReview.Status == StatusForDto.Completed)
                {
                    OperatorStatusCheck = StatusForDto.Completed;
                    ChangedOperatorCheckTime(operatorReview);
                    await CheckMechanicAcceptanceStatusValue();
                    return;
                }

                else if (operatorReview.Status == StatusForDto.Pending)
                {
                    OperatorStatusCheck = StatusForDto.Pending;
                    return;
                }

                OperatorStatusCheck = StatusForDto.Rejected;
                MechanicAcceptanceStatusCheck = StatusForDto.Rejected;
                ChangedOperatorCheckTime(operatorReview);
            }
        }

        private async Task CheckMechanicAcceptanceStatusValue()
        {
            var mechanicAcceptanceResponse = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesAsync(TodayDateForProgressBar);
            var mechanicAcceptance = mechanicAcceptanceResponse.Data.FirstOrDefault(x => x.DriverId == _driver.Id);

            if (mechanicAcceptance != null)
            {
                if (mechanicAcceptance.IsAccepted.Value && mechanicAcceptance.Status == StatusForDto.Completed)
                {
                    MechanicAcceptanceStatusCheck = StatusForDto.Completed;
                    ChangedMechanicAcceptanceCheckTime(mechanicAcceptance);
                    return;
                }

                else if (mechanicAcceptance.Status == StatusForDto.Pending)
                {
                    MechanicAcceptanceStatusCheck = StatusForDto.Pending;
                    return;
                }

                MechanicAcceptanceStatusCheck = StatusForDto.Rejected;
                ChangedMechanicAcceptanceCheckTime(mechanicAcceptance);
            }
        }

        #endregion

        #region Departments status check time methods

        private void ChangedDoctorCheckTime(DoctorReviewDto reviewDto)
        {
            DateTime utcDateTime = reviewDto.Date;
            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT0");
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, localTimeZone);
            DoctorCheckTime = localDateTime.ToString("HH : mm");
        }

        private void ChangedMechanicAcceptanceCheckTime(MechanicAcceptanceDto acceptanceDto)
        {
            DateTime utcDateTime = acceptanceDto.Date.Value;
            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT0");
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, localTimeZone);
            MechanicAcceptanceCheckTime = localDateTime.ToString("HH : mm");
        }

        private void ChangedOperatorCheckTime(OperatorReviewDto reviewDto)
        {
            DateTime utcDateTime = reviewDto.Date.Value;
            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT0");
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, localTimeZone);
            OperatorCheckTime = localDateTime.ToString("HH : mm");
        }

        private void ChangedMechanicHandoverCheckTime(MechanicHandoverDto handoverDto)
        {
            DateTime utcDateTime = handoverDto.Date;
            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT0");
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, localTimeZone);
            MechanicHandoverCheckTime = localDateTime.ToString("HH : mm");
        }

        #endregion

        #region Notification methods

        private async Task AcceptButton()
        {
            // Disable the buttons and change their colors
            IsAcceptButtonEnabled = false;
            IsRejectButtonEnabled = false;
            AcceptButtonColor = Color.Gray;
            RejectButtonColor = Color.Gray;

            var statusNumber = LocalStorage.GetSignalRData().statusNumber;

            switch (statusNumber)
            {
                case 0:
                    MechanicHandoverStatusCheck = StatusForDto.Completed;
                    break;
                case 1:
                    OperatorStatusCheck = StatusForDto.Completed;
                    break;
                case 2:
                    MechanicAcceptanceStatusCheck = StatusForDto.Completed;
                    break;
            }

            await _signalRService.SendResponse(true);
            await ClosePopup();
            await RefreshPage();
        }

        private async Task RejectButton()
        {
            // Disable the buttons and change their colors
            IsAcceptButtonEnabled = false;
            IsRejectButtonEnabled = false;
            AcceptButtonColor = Color.Gray;
            RejectButtonColor = Color.Gray;

            var statusNumber = LocalStorage.GetSignalRData().statusNumber;

            switch (statusNumber)
            {
                case 0:
                    MechanicHandoverStatusCheck = StatusForDto.Rejected;
                    break;
                case 1:
                    OperatorStatusCheck = StatusForDto.Rejected;
                    break;
                case 2:
                    MechanicAcceptanceStatusCheck = StatusForDto.Rejected;
                    break;
            }

            await _signalRService.SendResponse(false);
            await ClosePopup();
            await RefreshPage();
        }

        private async Task ClosePopup()
        {
            try
            {
                SecureStorage.Remove("popup_message");
                SecureStorage.Remove("popup_visible");

                if (PopupNavigation.Instance.PopupStack.Any())
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    Console.WriteLine("Popup yopildi.");
                }
                else
                {
                    Console.WriteLine("PopupStack bo'sh.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Popup yopishda xatolik: {ex.Message}");
                throw new Exception($"Popup yopilmayapti... {ex.Message}");
            }
        }

        private async Task RefreshPage()
        {
            await App.Current.MainPage.Navigation.PushAsync(new RoadMapPage());
        }

        #endregion
    }
}
