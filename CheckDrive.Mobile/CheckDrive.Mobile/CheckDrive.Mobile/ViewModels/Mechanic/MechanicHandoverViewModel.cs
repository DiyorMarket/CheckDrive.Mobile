using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Stores.Mechanic;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using CheckDrive.Mobile.Stores.Car;
using CheckDrive.Mobile.ViewModels.Mechanic.Popups;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Stores.CheckPoint;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Views.Mechanic.Popups;

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

        public DateTime CurrentDate { get; }

        public MechanicHandoverViewModel()
        {
            _checkPointStore = DependencyService.Get<ICheckPointStore>();
            _mechanicStore = DependencyService.Get<IMechanicStore>();
            _carStore = DependencyService.Get<ICarStore>();

            _cars = new List<CarDto>();
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
                var checkPointsTask = _checkPointStore.GetCheckPointsAsync(CheckPointStage.DoctorReview);
                var carsTask = _carStore.GetAvailableCarsAsync();

                await Task.WhenAll(checkPointsTask, carsTask);

                _allCheckPoints.AddRange(checkPointsTask.Result);
                FilteredCheckPoints.Clear();
                foreach (var checkPoint in checkPointsTask.Result)
                {
                    FilteredCheckPoints.Add(checkPoint);
                }

                _cars.Clear();
                _cars.AddRange(carsTask.Result);
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
            if (!_cars.Any())
            {
                await DisplayErrorAsync("Topshirish uchun bo'sh avtomobil topilmadi", "No avialable car for handover.");
                return;
            }

            var completionSource = new TaskCompletionSource<MechanicHandoverReview>();
            var reviewPopup = new MechanicHandoverReviewPopup
            {
                BindingContext = new MechanicHandoverReviewViewModel(checkPoint, _cars, completionSource)
            };

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(reviewPopup);

            var result = await completionSource.Task;
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();

            if (result != null)
            {
                await SendReviewAsync(result, checkPoint.DriverName);
            }
        }

        private async Task SendReviewAsync(MechanicHandoverReview review, string driverName)
        {
            IsBusy = true;

            try
            {
                await _mechanicStore.CreateReviewAsync(review);
                await DisplaySuccessAsync($"{driverName}ga {review.Car} topshirish so'rovi muvaffaqiyatli yuborildi.");
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
