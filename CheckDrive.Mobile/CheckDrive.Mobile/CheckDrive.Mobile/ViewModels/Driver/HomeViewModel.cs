using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Stores.Driver;
using Rg.Plugins.Popup.Services;
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

        public string CurrentDate { get; }
        public CheckPointDto CheckPoint { get; private set; }

        public Command RefreshCommand { get; }
        public Command<ReviewDto> ShowConfirmationPopupCommand { get; }

        public HomeViewModel()
        {
            _signalRService = DependencyService.Get<SignalRService>();
            _driverStore = DependencyService.Get<IDriverStore>();

            RefreshCommand = new Command(async () => await OnRefreshAsync());
            ShowConfirmationPopupCommand = new Command<ReviewDto>(async (review) => await OnShowConfirmationPopupAsync(review));

            CurrentDate = DateTime.Now.ToLocalTime().ToString("dddd, dd MMMM");
            Reviews = new ObservableCollection<ReviewDto>();

            SetupReviews();
        }

        public async Task InitializeAsync()
        {
            await _signalRService.StartConnectionAsync();
            SubscribeToCheckPointProgressUpdates();
        }

        public async Task OnRefreshAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var checkPoint = await _driverStore.GetCurrentCheckPointAsync();
                CheckPoint = checkPoint;

                UpdateReviews(checkPoint);
                UpdateCar(checkPoint);
                // await CheckPendingReviews(checkPoint);
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

        private void UpdateCar(CheckPointDto checkPoint)
        {
            var car = checkPoint.Car;
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
            MessagingCenter.Subscribe<SignalRService>(this, "CheckPointProgressUpdated", async _ =>
            {
                await OnRefreshAsync();
            });
        }

        private async Task OnShowConfirmationPopupAsync(ReviewDto review)
        {
            if (review.Status != ReviewStatus.Pending)
            {
                return;
            }

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                return;
            }

            await DisplayReviewConfirmationAsync(review, CheckPoint);
        }

        private async Task DisplayReviewConfirmationAsync(ReviewDto reviewToUpdate, CheckPointDto checkPoint)
        {
            try
            {
                var completionSource = new TaskCompletionSource<ReviewConfirmationRequest>();
                var popup = ReviewConfirmationFactory.GetConfirmationPopup(completionSource, checkPoint, reviewToUpdate.Type);

                await PopupNavigation.Instance.PushAsync(popup);

                var request = await completionSource.Task;

                await _driverStore.SendReviewConfirmationAsync(request);
                await OnRefreshAsync();
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
