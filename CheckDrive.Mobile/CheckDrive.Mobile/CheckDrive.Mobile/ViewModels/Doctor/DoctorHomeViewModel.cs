using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Stores.Doctor;
using CheckDrive.Mobile.Views.Doctor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Doctor
{
    public class DoctorHomeViewModel : BaseViewModel
    {
        private readonly IDoctorStore _doctorStore;

        private readonly List<DriverDto> _drivers;
        public ObservableCollection<DriverDto> Drivers { get; }

        public Command<string> SearchCommand { get; }
        public Command RefreshCommand { get; }
        public Command<DriverDto> ShowReviewPopupCommand { get; }


        public DateTime CurrentDate { get; }

        public DoctorHomeViewModel()
        {
            _doctorStore = DependencyService.Get<IDoctorStore>();

            Drivers = new ObservableCollection<DriverDto>();
            _drivers = new List<DriverDto>();
            CurrentDate = DateTime.Now;

            SearchCommand = new Command<string>(OnSearch);
            RefreshCommand = new Command(async () => await LoadDriversAsync());
            ShowReviewPopupCommand = new Command<DriverDto>(async (driver) => await ShowReviewPopup(driver));
        }

        public async Task LoadDriversAsync()
        {
            IsRefreshing = true;

            try
            {
                var drivers = await _doctorStore.GetDriversAsync();

                _drivers.Clear();
                _drivers.AddRange(drivers);

                Drivers.Clear();
                foreach (var driver in drivers)
                {
                    Drivers.Add(driver);
                }
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Xato yuz berdi", ex.Message);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private void OnSearch(string search)
        {
            Drivers.Clear();

            if (string.IsNullOrWhiteSpace(search))
            {
                foreach (var driver in _drivers)
                {
                    Drivers.Add(driver);
                }

                return;
            }

            var filteredDrivers = _drivers.Where(x => x.FullName.ToLower().Contains(search.ToLower()));

            foreach (var driver in filteredDrivers)
            {
                Drivers.Add(driver);
            }
        }

        private async Task ShowReviewPopup(DriverDto driver)
        {
            var completionSource = new TaskCompletionSource<DoctorReview>();
            var reviewPopup = new DoctorReviewPopup
            {
                BindingContext = new DoctorReviewViewModel(driver, completionSource)
            };

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(reviewPopup);

            var result = await completionSource.Task;
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();

            if (result != null)
            {
                await SendReviewAsync(result, driver.FullName);
            }
        }

        private async Task SendReviewAsync(DoctorReview review, string driverName)
        {
            IsBusy = true;

            try
            {
                await _doctorStore.CreateAsync(review);
                await DisplaySuccessAsync($"{driverName} uchun tekshiruv muvaffaqiyatli saqlandi.");

                var driver = _drivers.Find(x => x.Id == review.DriverId);
                _drivers.Remove(driver);
                Drivers.Remove(driver);
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
