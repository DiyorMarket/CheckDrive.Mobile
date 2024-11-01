using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Stores.Driver;
using CheckDrive.Mobile.ViewModels.Driver.Popups;
using CheckDrive.Mobile.Views.Driver.Popups;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Driver
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly SignalRService _signalRService;
        private readonly IDriverStore _driverStore;

        public ObservableCollection<ReviewDto> Reviews { get; }

        public Command RefreshCommand { get; }

        public string CurrentDate { get; }

        private decimal _monthlyDistanceLimit;
        public decimal MonthlyDistanceLimit
        {
            get => _monthlyDistanceLimit;
            set => SetProperty(ref _monthlyDistanceLimit, value);
        }

        private decimal _currentMonthMileage;
        public decimal CurrentMonthMileage
        {
            get => _currentMonthMileage;
            set => SetProperty(ref _currentMonthMileage, value);
        }

        private decimal _mileageLimitProgress;
        public decimal MileageLimitProgress
        {
            get => _mileageLimitProgress;
            set => SetProperty(ref _mileageLimitProgress, value);
        }

        public HomeViewModel()
        {
            _signalRService = DependencyService.Get<SignalRService>();
            _driverStore = DependencyService.Get<IDriverStore>();

            RefreshCommand = new Command(async () => await OnRefreshAsync(true));

            CurrentDate = DateTime.Now.ToLocalTime().ToString("dddd, dd MMMM");
            Reviews = new ObservableCollection<ReviewDto>();

            SetupReviews();
        }

        public async Task InitializeAsync()
        {
            await _signalRService.StartConnectionAsync();
            SubscribeToCheckPointProgressUpdates();
        }

        public async Task OnRefreshAsync(bool forceRefresh = false)
        {
            IsBusy = true;

            try
            {
                var checkPoint = await _driverStore.GetCurrentCheckPointAsync();

                UpdateReviews(checkPoint);
                UpdateCar(checkPoint.Car);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SetupReviews()
        {
            foreach (var type in Enum.GetValues(typeof(ReviewType)))
            {
                var review = new ReviewDto
                {
                    Type = (ReviewType)type,
                    Status = (ReviewType)type == ReviewType.DoctorReview ? ReviewStatus.InProgress : ReviewStatus.NotStarted,
                };

                Reviews.Add(review);
            }
        }

        private void UpdateReviews(CheckPointDto checkPoint)
        {
            if (checkPoint is null)
            {
                return;
            }

            checkPoint.SetupReviews();
            var reviewDictionary = checkPoint.Reviews.ToDictionary(r => r.Type);

            for (int i = 0; i < Reviews.Count; i++)
            {
                if (reviewDictionary.TryGetValue(Reviews[i].Type, out var reviewToUpdate))
                {
                    Reviews[i].Update(reviewToUpdate);
                }
                else if (i > 0 && Reviews[i - 1].Status == ReviewStatus.Approved)
                {
                    Reviews[i].Update(ReviewStatus.InProgress);
                }
            }
        }

        private void UpdateCar(CarDto car)
        {
            if (car == null)
            {
                return;
            }

            MonthlyDistanceLimit = car.MonthlyDistanceLimit;
            CurrentMonthMileage = car.CurrentMonthMileage;
            MileageLimitProgress = car.MileageLimitProgress;
        }

        private void SubscribeToCheckPointProgressUpdates()
        {
            MessagingCenter.Subscribe<SignalRService, ReviewDto>(this, "NotifyDoctorReview", (sender, request) =>
            {
                var reviewToUpdate = Reviews.First(x => x.Type == request.Type);
                reviewToUpdate.Update(request);
                var mechanicHandover = Reviews.First(x => x.Type == ReviewType.MechanicHandover);
                mechanicHandover.Update(ReviewStatus.InProgress);
            });

            MessagingCenter.Subscribe<SignalRService, MechanicHandoverReview>(this, "MechanicHandoverConfirmation", async (sender, request) =>
            {
                await HandleConfirmationAsync(request.GetReviewConfirmationMessage(), request.CheckPointId, ReviewType.MechanicHandover);
            });

            MessagingCenter.Subscribe<SignalRService, OperatorReview>(this, "OperatorReviewConfirmation", async (sender, request) =>
            {
                await HandleConfirmationAsync(request.GetReviewConfirmationMessage(), request.CheckPointId, ReviewType.OperatorReview);
            });

            MessagingCenter.Subscribe<SignalRService, MechanicAcceptanceReview>(this, "MechanicAcceptanceConfirmation", async (sender, request) =>
            {
                await HandleConfirmationAsync(request.GetReviewConfirmationMessage(), request.CheckPointId, ReviewType.MechanicAcceptance);
            });
        }

        private async Task HandleConfirmationAsync(string message, int checkPointId, ReviewType reviewType)
        {
            IsBusy = true;

            try
            {
                var completionSource = new TaskCompletionSource<bool>();
                var popup = new ReviewConfirmationPopup()
                {
                    BindingContext = new ReviewConfirmationViewModel(message, completionSource)
                };

                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);

                var isAccepted = await completionSource.Task;
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();

                var confirmation = new DriverReviewResponse
                {
                    CheckPointId = checkPointId,
                    IsAcceptedByDriver = isAccepted,
                    Notes = "",
                    ReviewType = reviewType,
                };

                await _driverStore.SendReviewConfirmationAsync(confirmation);
                await OnRefreshAsync(true);
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("So'rovni tasdiqlashda xato ro'y berdi.", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
