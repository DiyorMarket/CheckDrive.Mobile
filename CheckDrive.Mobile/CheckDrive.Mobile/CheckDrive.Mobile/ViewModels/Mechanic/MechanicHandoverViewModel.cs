using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Stores.Mechanic;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using CheckDrive.Mobile.Stores.Car;
using CheckDrive.Mobile.Stores.CheckPoint;
using CheckDrive.Mobile.Views.Mechanic.Popups;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Mechanic.Handover;
using Rg.Plugins.Popup.Services;

namespace CheckDrive.Mobile.ViewModels.Mechanic
{
    public class MechanicHandoverViewModel : BaseViewModel
    {
        private readonly ICheckPointStore _checkPointStore;
        private readonly IMechanicStore _mechanicStore;
        private readonly ICarStore _carStore;

        private readonly List<CarDto> _cars;
        private readonly List<CheckPointDto> _allCheckPoints;
        public ObservableCollection<CheckPointDto> FilteredCheckPoints { get; }

        public Command<string> SearchCommand { get; }
        public Command RefreshCommand { get; }
        public Command<CheckPointDto> ShowReviewPopupCommand { get; }

        public string CurrentDate { get; }

        public MechanicHandoverViewModel()
        {
            _checkPointStore = DependencyService.Get<ICheckPointStore>();
            _mechanicStore = DependencyService.Get<IMechanicStore>();
            _carStore = DependencyService.Get<ICarStore>();

            _cars = new List<CarDto>();
            _allCheckPoints = new List<CheckPointDto>();
            FilteredCheckPoints = new ObservableCollection<CheckPointDto>();

            CurrentDate = DateTime.Now.ToString("dd MMMM");

            SearchCommand = new Command<string>(OnSearch);
            RefreshCommand = new Command(async () => await LoadDriversAsync());
            ShowReviewPopupCommand = new Command<CheckPointDto>(async (driver) => await ShowReviewPopup(driver));
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
                var checkPoints = await _checkPointStore.GetCheckPointsAsync(CheckPointStage.DoctorReview);
                var cars = await _carStore.GetAvailableCarsAsync();

                _allCheckPoints.AddRange(checkPoints);
                FilteredCheckPoints.Clear();
                foreach (var checkPoint in checkPoints)
                {
                    FilteredCheckPoints.Add(checkPoint);
                }

                _cars.Clear();
                _cars.AddRange(cars);
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
            var filteredCheckPoints = string.IsNullOrEmpty(searchText)
                ? _allCheckPoints
                : _allCheckPoints.Where(x => x.DriverName.Contains(search));

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

        private async Task ShowReviewPopup(CheckPointDto checkPoint)
        {
            if (!CanShowReviewPopup())
            {
                return;
            }

            var completionSource = new TaskCompletionSource<MechanicHandoverRequest>();
            var reviewPopup = new MechanicHandoverReviewPopup(checkPoint, _cars, completionSource);

            MechanicHandoverRequest result = null;

            try
            {
                await PopupNavigation.Instance.PushAsync(reviewPopup);

                result = await completionSource.Task;

                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Tekshiruv oynasini ochishda muammo ro'y berdi.", ex.Message);
            }

            if (result != null)
            {
                await SendReviewAsync(result);
            }
        }

        private async Task SendReviewAsync(MechanicHandoverRequest request)
        {
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

        private bool CanShowReviewPopup()
            => _cars.Any() && PopupNavigation.Instance.PopupStack.Count == 0;
    }
}
