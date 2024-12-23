using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Operator;
using CheckDrive.Mobile.Stores.CheckPoint;
using CheckDrive.Mobile.Stores.Operator;
using CheckDrive.Mobile.Views.Operator;
using Rg.Plugins.Popup.Services;
using CheckDrive.Mobile.Services;

namespace CheckDrive.Mobile.ViewModels.Operator
{
    public class OperatorHomeViewModel : BaseViewModel
    {
        private readonly SignalRService _signalRService;
        private readonly ICheckPointStore _checkPointStore;
        private readonly IOperatorStore _operatorStore;

        private readonly List<CheckPointDto> _allCheckPoints;
        public ObservableCollection<CheckPointDto> FilteredCheckPoints { get; }
        public List<OilMark> OilMarks { get; }

        public Command<CheckPointDto> ShowReviewPopupCommand { get; }
        public Command<string> SearchCommand { get; }
        public Command RefreshCommand { get; }

        public string CurrentDate { get; }

        public OperatorHomeViewModel()
        {
            _signalRService = DependencyService.Get<SignalRService>();
            _checkPointStore = DependencyService.Get<ICheckPointStore>();
            _operatorStore = DependencyService.Get<IOperatorStore>();

            _allCheckPoints = new List<CheckPointDto>();
            FilteredCheckPoints = new ObservableCollection<CheckPointDto>();
            OilMarks = new List<OilMark>();

            CurrentDate = DateTime.Now.ToString("dd MMMM");

            ShowReviewPopupCommand = new Command<CheckPointDto>(async (checkPoint) => await OnShowReviewPopupAsync(checkPoint));
            SearchCommand = new Command<string>(OnSearch);
            RefreshCommand = new Command(async () => await LoadDataAsync());
        }

        public async Task InitializeAsync()
        {
            await _signalRService.StartConnectionAsync();
            SubscribeToCheckPointProgressUpdates();
        }

        public async Task LoadDataAsync()
        {
            IsBusy = true;

            try
            {
                var checkPoints = await _checkPointStore.GetCheckPointsAsync(CheckPointStage.MechanicHandover);
                var oilMarks = await _operatorStore.GetOilMarksAsync();

                _allCheckPoints.Clear();
                _allCheckPoints.AddRange(checkPoints);

                OilMarks.Clear();
                OilMarks.AddRange(oilMarks);

                UpdateCheckPoints(checkPoints);
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

        private async Task OnShowReviewPopupAsync(CheckPointDto checkPoint)
        {
            if (!OilMarks.Any())
            {
                await DisplayErrorAsync("Yoqilg'i markalari topilmadi.", "Cannot perform operator review without oil marks.");
                return;
            }

            try
            {
                var completionSource = new TaskCompletionSource<OperatorReviewRequest>();
                var reviewPopup = new OperatorReviewPopup(checkPoint, OilMarks, completionSource);

                await PopupNavigation.Instance.PushAsync(reviewPopup);

                var result = await completionSource.Task;

                await SendReviewAsync(result);
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Tekshiruv oynasini ochishda xato ro'y berdi.", ex.Message);
            }
        }

        private void OnSearch(string searchText)
        {
            searchText = searchText.Trim().ToLower();
            var filteredCheckPoints = string.IsNullOrEmpty(searchText)
                ? _allCheckPoints
                : _allCheckPoints.Where(x => x.DriverName.ToLower().Contains(searchText));

            UpdateCheckPoints(filteredCheckPoints);
        }

        private async Task SendReviewAsync(OperatorReviewRequest request)
        {
            if (request == null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await _operatorStore.CreateReviewAsync(request);

                await DisplayReviewSuccessAsync();

                var checkPointToRemove = _allCheckPoints.Find(x => x.Id == request.CheckPointId);
                _allCheckPoints.Remove(checkPointToRemove);
                FilteredCheckPoints.Remove(checkPointToRemove);
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

        private void UpdateCheckPoints(IEnumerable<CheckPointDto> checkPoints)
        {
            FilteredCheckPoints.Clear();

            foreach (var checkPoint in checkPoints)
            {
                FilteredCheckPoints.Add(checkPoint);
            }
        }

        private void SubscribeToCheckPointProgressUpdates()
        {
            MessagingCenter.Subscribe<SignalRService>(this, "CheckPointProgressUpdated", async _ =>
            {
                await LoadDataAsync();
            });
        }
    }
}
