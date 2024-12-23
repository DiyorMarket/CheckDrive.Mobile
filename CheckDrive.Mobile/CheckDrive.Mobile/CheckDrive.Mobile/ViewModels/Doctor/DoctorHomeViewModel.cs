using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Stores.Doctor;
using CheckDrive.Mobile.Stores.Driver;
using CheckDrive.Mobile.Views.Doctor;
using Rg.Plugins.Popup.Services;
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
        private readonly IDriverStore _driverStore;

        private readonly List<DriverDto> _drivers;
        public ObservableCollection<DriverDto> Drivers { get; }

        public Command<string> SearchCommand { get; }
        public Command RefreshCommand { get; }
        public Command<DriverDto> ShowReviewPopupCommand { get; }

        public string CurrentDate { get; }

        public DoctorHomeViewModel()
        {
            _doctorStore = DependencyService.Get<IDoctorStore>();
            _driverStore = DependencyService.Get<IDriverStore>();

            Drivers = new ObservableCollection<DriverDto>();
            _drivers = new List<DriverDto>();
            CurrentDate = DateTime.Now.ToString("dddd, dd MMMM");

            SearchCommand = new Command<string>(OnSearch);
            RefreshCommand = new Command(async () => await LoadDriversAsync());
            ShowReviewPopupCommand = new Command<DriverDto>(async (driver) => await ShowReviewPopup(driver));
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
                var drivers = await _driverStore.GetDriversAsync();

                _drivers.Clear();
                _drivers.AddRange(drivers);

                UpdateDrivers(drivers);
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Xato yuz berdi", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();
            var filteredDrivers = string.IsNullOrEmpty(searchText)
                ? _drivers
                : _drivers.Where(x => x.FullName.ToLower().Contains(searchText));

            UpdateDrivers(filteredDrivers);
        }

        private async Task ShowReviewPopup(DriverDto driver)
        {
            if (driver is null)
            {
                return;
            }

            try
            {
                var completionSource = new TaskCompletionSource<DoctorReviewRequest>();
                var reviewPopup = new DoctorReviewPopup(driver, completionSource);

                await PopupNavigation.Instance.PushAsync(reviewPopup);

                var result = await completionSource.Task;

                await SendReviewAsync(result);
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Tekshiruv oynasini ochishda muammo ro'y berdi.", ex.Message);
            }
        }

        private async Task SendReviewAsync(DoctorReviewRequest request)
        {
            if (request is null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await _doctorStore.CreateAsync(request);

                await DisplayReviewSuccessAsync();

                var driver = _drivers.Find(x => x.Id == request.DriverId);

                if (driver != null)
                {
                    _drivers.Remove(driver);
                    Drivers.Remove(driver);
                }
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

        private void UpdateDrivers(IEnumerable<DriverDto> drivers)
        {
            Drivers.Clear();

            foreach (var driver in drivers)
            {
                Drivers.Add(driver);
            }
        }
    }
}
