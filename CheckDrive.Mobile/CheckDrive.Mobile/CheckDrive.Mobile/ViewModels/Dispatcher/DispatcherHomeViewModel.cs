using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Dispatcher;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Stores.CheckPoint;
using CheckDrive.Mobile.Stores.Dispatcher;
using CheckDrive.Mobile.Views.Dispatcher.Popups;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Dispatcher
{
    public class DispatcherHomeViewModel : BaseViewModel
    {
        private readonly ICheckPointStore _checkPointStore;
        private readonly IDispatcherStore _dispatcherStore;

        private readonly List<CheckPointDto> _allCheckPoints;
        public ObservableCollection<CheckPointDto> FilteredCheckPoints { get; }

        public Command<CheckPointDto> ShowReviewCommand { get; }
        public Command<string> SearchCommand { get; }
        public Command RefreshCommand { get; }

        public string CurrentDate { get; }

        public DispatcherHomeViewModel()
        {
            _checkPointStore = DependencyService.Get<ICheckPointStore>();
            _dispatcherStore = DependencyService.Get<IDispatcherStore>();

            CurrentDate = DateTime.Now.ToString("dd MMMM");

            _allCheckPoints = new List<CheckPointDto>();
            FilteredCheckPoints = new ObservableCollection<CheckPointDto>();

            RefreshCommand = new Command(async () => await LoadCheckPointsAsync());
            ShowReviewCommand = new Command<CheckPointDto>(async (checkPoint) => await OnShowReviewAsync(checkPoint));
            SearchCommand = new Command<string>(OnSearch);
        }

        public async Task LoadCheckPointsAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var checkPoints = await _checkPointStore.GetCheckPointsAsync(CheckPointStage.MechanicAcceptance);

                _allCheckPoints.Clear();
                _allCheckPoints.AddRange(checkPoints);

                FilteredCheckPoints.Clear();
                foreach (var checkPoint in checkPoints)
                {
                    FilteredCheckPoints.Add(checkPoint);
                }
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Tekshiruv uchun haydovchilarni yuklashda xato ro'y berdi.", ex.Message);
            }
            finally
            {
                IsBusy = false;
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

                return;
            }

            var filteredCheckPoints = _allCheckPoints.Where(x => x.DriverName.Contains(searchText)).ToList();

            foreach (var checkPoint in filteredCheckPoints)
            {
                FilteredCheckPoints.Add(checkPoint);
            }
        }

        private async Task OnShowReviewAsync(CheckPointDto checkPoint)
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                return;
            }

            try
            {
                var completionSource = new TaskCompletionSource<DispatcherReviewRequest>();
                var reviewPopup = new DispatcherReviewPopup(checkPoint, completionSource);

                await PopupNavigation.Instance.PushAsync(reviewPopup);

                var result = await completionSource.Task;

                await SendReviewAsync(result);
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Tekshiruv oynasini ochishda xato ro'y berdi.", ex.Message);
            }
        }

        private async Task SendReviewAsync(DispatcherReviewRequest request)
        {
            if (request is null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await _dispatcherStore.CreateReveiwAsync(request);

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
