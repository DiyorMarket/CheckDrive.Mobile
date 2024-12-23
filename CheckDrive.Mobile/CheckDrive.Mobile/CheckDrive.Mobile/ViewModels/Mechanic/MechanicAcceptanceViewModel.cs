using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Stores.CheckPoint;
using CheckDrive.Mobile.Stores.Mechanic;
using CheckDrive.Mobile.Views.Mechanic.Popups;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using Xamarin.Forms;
using System.Linq;
using CheckDrive.Mobile.Models.Mechanic.Acceptance;
using Rg.Plugins.Popup.Services;
using CheckDrive.Mobile.Services;

namespace CheckDrive.Mobile.ViewModels.Mechanic
{
    public class MechanicAcceptanceViewModel : BaseViewModel
    {
        private readonly SignalRService _signalRService;
        private readonly ICheckPointStore _checkPointStore;
        private readonly IMechanicStore _mechanicStore;

        private readonly List<CheckPointDto> _allCheckPoints;
        public ObservableCollection<CheckPointDto> FilteredCheckPoints { get; }

        public Command<string> SearchCommand { get; }
        public Command RefreshCommand { get; }
        public Command<CheckPointDto> ShowReviewPopupCommand { get; }

        public string CurrentDate { get; }

        public MechanicAcceptanceViewModel()
        {
            _signalRService = DependencyService.Get<SignalRService>();
            _checkPointStore = DependencyService.Get<ICheckPointStore>();
            _mechanicStore = DependencyService.Get<IMechanicStore>();

            _allCheckPoints = new List<CheckPointDto>();
            FilteredCheckPoints = new ObservableCollection<CheckPointDto>();

            CurrentDate = DateTime.Now.ToString("dd MMMM");

            SearchCommand = new Command<string>(OnSearch);
            RefreshCommand = new Command(async () => await LoadDriversAsync());
            ShowReviewPopupCommand = new Command<CheckPointDto>(async (checkPoint) => await ShowReviewPopup(checkPoint));
        }

        public async Task InitializeAsync()
        {
            await _signalRService.StartConnectionAsync();
            SubscribeToCheckPointProgressUpdates();
        }

        public async Task LoadDriversAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var checkPoints = await _checkPointStore.GetCheckPointsAsync(CheckPointStage.OperatorReview);

                _allCheckPoints.Clear();
                _allCheckPoints.AddRange(checkPoints);

                UpdateCheckPoints(checkPoints);
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Kutilmagan xato ro'y berdi.", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnSearch(string search)
        {
            var searchText = search.ToLower().Trim();
            var filteredCheckPoints = string.IsNullOrWhiteSpace(searchText)
                ? _allCheckPoints
                : _allCheckPoints.Where(x => x.DriverName.Contains(searchText));

            UpdateCheckPoints(filteredCheckPoints);
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
                await LoadDriversAsync();
            });
        }

        private async Task ShowReviewPopup(CheckPointDto checkPoint)
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                return;
            }

            try
            {
                var completionSource = new TaskCompletionSource<MechanicAcceptanceRequest>();
                var reviewPopup = new MechanicAcceptanceReviewPopup(checkPoint, completionSource);

                await PopupNavigation.Instance.PushAsync(reviewPopup);

                var result = await completionSource.Task;

                await SendReviewAsync(result);
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Tekshiruv oynasini ochishda xato ro'y berdi.", ex.Message);
            }
        }

        private async Task SendReviewAsync(MechanicAcceptanceRequest request)
        {
            if (request is null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await _mechanicStore.CreateReviewAsync(request);

                await DisplayReviewSuccessAsync();

                var checkPointToRemove = _allCheckPoints.Find(x => x.Id == request.CheckPointId);
                _allCheckPoints.Remove(checkPointToRemove);
                FilteredCheckPoints.Remove(checkPointToRemove);
            }
            catch (Exception ex)
            {
                await DisplayReviewErrorAsync(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
