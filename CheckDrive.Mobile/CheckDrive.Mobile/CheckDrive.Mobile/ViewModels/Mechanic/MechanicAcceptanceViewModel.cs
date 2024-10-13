using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Review;
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
using CheckDrive.Mobile.ViewModels.Mechanic.Popups;

namespace CheckDrive.Mobile.ViewModels.Mechanic
{
    public class MechanicAcceptanceViewModel : BaseViewModel
    {
        private readonly ICheckPointStore _checkPointStore;
        private readonly IMechanicHandoverStore _mechanicStore;

        private readonly List<CheckPointDto> _allCheckPoints;
        public ObservableCollection<CheckPointDto> FilteredCheckPoints { get; }

        public Command<string> SearchCommand { get; }
        public Command RefreshCommand { get; }
        public Command<CheckPointDto> ShowReviewPopupCommand { get; }

        public DateTime CurrentDate { get; }

        public MechanicAcceptanceViewModel()
        {
            _checkPointStore = DependencyService.Get<ICheckPointStore>();
            _mechanicStore = DependencyService.Get<IMechanicHandoverStore>();

            _allCheckPoints = new List<CheckPointDto>();
            FilteredCheckPoints = new ObservableCollection<CheckPointDto>();

            CurrentDate = DateTime.Now;

            SearchCommand = new Command<string>(OnSearch);
            RefreshCommand = new Command(async () => await LoadDriversAsync());
            ShowReviewPopupCommand = new Command<CheckPointDto>(async (checkPoint) => await ShowReviewPopup(checkPoint));
        }

        public async Task LoadDriversAsync()
        {
            IsRefreshing = true;

            try
            {
                var checkPoints = await _checkPointStore.GetCheckPointsAsync(CheckPointStage.OperatorReview);

                _allCheckPoints.AddRange(checkPoints);
                FilteredCheckPoints.Clear();
                foreach (var checkPoint in checkPoints)
                {
                    FilteredCheckPoints.Add(checkPoint);
                }
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Kutilmagan xato ro'y berdi.", ex.Message);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private void OnSearch(string search)
        {
            FilteredCheckPoints.Clear();

            if (string.IsNullOrWhiteSpace(search))
            {
                foreach (var checkPoint in _allCheckPoints)
                {
                    FilteredCheckPoints.Add(checkPoint);
                }

                return;
            }

            var filteredCheckPoints = _allCheckPoints
                .Where(x => x.DriverName.ToLower().Contains(search.ToLower()))
                .OrderBy(x => x.DriverName);

            foreach (var checkPoint in filteredCheckPoints)
            {
                FilteredCheckPoints.Add(checkPoint);
            }
        }

        private async Task ShowReviewPopup(CheckPointDto checkPoint)
        {
            var completionSource = new TaskCompletionSource<MechanicAcceptanceReview>();
            var reviewPopup = new MechanicAcceptanceReviewPopup
            {
                BindingContext = new MechanicAcceptanceReviewViewModel(checkPoint, completionSource)
            };

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(reviewPopup);

            var result = await completionSource.Task;
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();

            if (result != null)
            {
                await SendReviewAsync(result, checkPoint.DriverName, checkPoint.Car.ToString());
            }
        }

        private async Task SendReviewAsync(MechanicAcceptanceReview review, string driverName, string carName)
        {
            IsBusy = true;

            try
            {
                await _mechanicStore.CreateReviewAsync(review);
                await DisplaySuccessAsync($"{driverName}dan {carName} qabul qilish so'rovi muvaffaqiyatli yuborildi.");
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync($"{driverName} uchun tekshiruvni saqlashda kutilmagan xato ro'y berdi. Iltimos qayta urunib ko'ring yoki texnik yordam bilan bog'laning.", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
