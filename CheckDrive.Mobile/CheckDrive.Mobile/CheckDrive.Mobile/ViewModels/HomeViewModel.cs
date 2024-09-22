using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Stores.Review;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IReviewStore _reviewStore;

        public ICommand RefreshCommand { get; }

        public string Date { get; }

        private CheckPointDto _checkPoint;
        public CheckPointDto CheckPoint
        {
            get => _checkPoint;
            set => SetProperty(ref _checkPoint, value);
        }

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

        private int _mileageLimitProgress;
        public int MileageLimitProgress
        {
            get => _mileageLimitProgress;
            set => SetProperty(ref _mileageLimitProgress, value);
        }

        public ObservableCollection<ReviewDto> Reviews { get; private set; }

        public HomeViewModel()
        {
            _reviewStore = DependencyService.Get<IReviewStore>();
            RefreshCommand = new Command(async () => await LoadData(true));

            Date = DateTime.Now.ToLocalTime().ToString("dddd, dd MMMM");
            Reviews = new ObservableCollection<ReviewDto>();
        }

        public async Task LoadData(bool forceRefresh = false)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            Reviews.Clear();

            try
            {
                var checkPoint = await _reviewStore.GetCheckPointAsync(forceRefresh);

                CheckPoint = checkPoint;
                SetCarProperties(checkPoint.Car);

                foreach (var review in checkPoint.Reviews)
                {
                    Reviews.Add(review);
                }
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

        private void SetCarProperties(CarDto car)
        {
            if (car == null)
            {
                return;
            }

            MonthlyDistanceLimit = car.MonthlyDistanceLimit;
            CurrentMonthMileage = car.CurrentMonthMileage;
            MileageLimitProgress = (int)((car.CurrentMonthMileage * 100) / car.MonthlyDistanceLimit);
        }
    }
}
