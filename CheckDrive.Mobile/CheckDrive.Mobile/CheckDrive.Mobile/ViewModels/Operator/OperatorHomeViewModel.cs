using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Stores.CheckPoint;
using CheckDrive.Mobile.Stores.Operator;
using CheckDrive.Mobile.Views.Operator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Operator
{
    public class OperatorHomeViewModel : BaseViewModel
    {
        private readonly ICheckPointStore _checkPointStore;
        private readonly IOperatorStore _operatorStore;

        private readonly List<CheckPointDto> _allCheckPoints;
        public ObservableCollection<CheckPointDto> FilteredCheckPoints { get; }
        public List<OilMark> OilMarks { get; }

        public Command<CheckPointDto> ShowReviewPopupCommand { get; }
        public Command<string> SearchCommand { get; }
        public Command RefreshCommand { get; }

        public DateTime CurrentDate { get; }

        public OperatorHomeViewModel()
        {
            _checkPointStore = DependencyService.Get<ICheckPointStore>();
            _operatorStore = DependencyService.Get<IOperatorStore>();

            _allCheckPoints = new List<CheckPointDto>();
            FilteredCheckPoints = new ObservableCollection<CheckPointDto>();
            OilMarks = new List<OilMark>();

            CurrentDate = DateTime.Now;

            ShowReviewPopupCommand = new Command<CheckPointDto>(async (checkPoint) => await ShowReviewPopup(checkPoint));
            SearchCommand = new Command<string>(OnSearch);
            RefreshCommand = new Command(async () => await LoadData());
        }

        public async Task LoadData()
        {
            IsBusy = true;

            try
            {
                var checkPoints = await _checkPointStore.GetCheckPointsAsync(CheckPointStage.MechanicHandover);
                var oilMarks = await _operatorStore.GetOilMarksAsync();

                _allCheckPoints.Clear();
                _allCheckPoints.AddRange(checkPoints);

                foreach (var checkPoint in checkPoints)
                {
                    FilteredCheckPoints.Add(checkPoint);
                }

                OilMarks.Clear();
                OilMarks.AddRange(oilMarks);
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Ma'lumotlarni yuklashda kutilmagan xato ro'y berdi.", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ShowReviewPopup(CheckPointDto checkPoint)
        {
            if (!OilMarks.Any())
            {
                await DisplayErrorAsync("Yoqilg'i markalari topilmadi.", "");
                return;
            }

            try
            {
                var completionSource = new TaskCompletionSource<OperatorReview>();
                var reviewPopup = new OperatorReviewPopup
                {
                    BindingContext = new OperatorReviewViewModel(checkPoint, OilMarks, completionSource)
                };
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(reviewPopup);

                var result = await completionSource.Task;
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();

                if (result != null)
                {
                    await SendReviewAsync(result);
                }
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Tekshiruv oynasini ochishda xato ro'y berdi.", ex.Message);
            }
        }

        private void OnSearch(string searchText)
        {
            searchText = searchText.Trim().ToLower();

            FilteredCheckPoints.Clear();
            if (string.IsNullOrEmpty(searchText))
            {
                foreach (var checkPoint in _allCheckPoints)
                {
                    FilteredCheckPoints.Add(checkPoint);
                }
            }

            var filteredCheckPoints = _allCheckPoints
                .Where(x => x.DriverName.ToLower().Contains(searchText))
                .OrderBy(x => x.DriverName);

            foreach (var checkPoint in filteredCheckPoints)
            {
                FilteredCheckPoints.Add(checkPoint);
            }
        }

        private async Task SendReviewAsync(OperatorReview review)
        {
            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }

            IsBusy = true;
            try
            {
                await _operatorStore.CreateReviewAsync(review);
                await DisplaySuccessAsync($"Haydovchiga yoqilg'i quyish so'rovi muvaffaqiyatli yuborildi.");

                var checkPointToDelete = _allCheckPoints.First(x => x.Id == review.CheckPointId);
                _allCheckPoints.Remove(checkPointToDelete);

                checkPointToDelete = FilteredCheckPoints.First(x => x.Id == review.CheckPointId);
                FilteredCheckPoints.Remove(checkPointToDelete);
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("So'rovni bajarishda kutilmagan xato ro'y berdi.", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
