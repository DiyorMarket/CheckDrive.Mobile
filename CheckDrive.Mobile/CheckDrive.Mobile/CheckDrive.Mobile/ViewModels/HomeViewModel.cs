using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Stores.Review;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IReviewStore _reviewStore;

        public string Date => DateTime.Now.ToLocalTime().ToString("dd-MMM-yyyy");

        private CheckPointDto _checkPoint;
        public CheckPointDto CheckPoint
        {
            get => _checkPoint;
            set => SetProperty(ref _checkPoint, value);
        }

        public ObservableCollection<ReviewDto> Reviews { get; private set; }

        public HomeViewModel()
        {
            _reviewStore = DependencyService.Get<IReviewStore>();

            Reviews = new ObservableCollection<ReviewDto>();
        }

        public async Task LoadData()
        {
            IsBusy = true;
            Reviews.Clear();

            try
            {
                var checkPoint = await _reviewStore.GetCheckPointAsync();

                CheckPoint = checkPoint;

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
    }
}
