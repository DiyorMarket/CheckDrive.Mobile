using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Mechanic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Mechanic.Popups
{
    public class MechanicHistoryFilterViewModel : BaseViewModel
    {
        private readonly TaskCompletionSource<MechanicFilter> _completionSource;

        public ObservableCollection<PickerItem<int?>> Drivers { get; }
        public ObservableCollection<PickerItem<int?>> Cars { get; }
        public ObservableCollection<PickerItem<ReviewStatus?>> Statuses { get; }
        public ObservableCollection<PickerItem<string>> SortOptions { get; }

        public Command ApplyCommand { get; set; }
        public Command ResetCommand { get; set; }

        private PickerItem<int?> _selectedDriver;
        public PickerItem<int?> SelectedDriver
        {
            get => _selectedDriver;
            set => SetProperty(ref _selectedDriver, value);
        }

        private PickerItem<int?> _selectedCar;
        public PickerItem<int?> SelectedCar
        {
            get => _selectedCar;
            set => SetProperty(ref _selectedCar, value);
        }

        private PickerItem<ReviewStatus?> _selectedStatus;
        public PickerItem<ReviewStatus?> SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }

        private PickerItem<string> _selectedSort;
        public PickerItem<string> SelectedSort
        {
            get => _selectedSort;
            set => SetProperty(ref _selectedSort, value);
        }

        private DateTime _minDate;
        public DateTime MinDate
        {
            get => _minDate;
            set => SetProperty(ref _minDate, value);
        }

        private DateTime _maxDate;
        public DateTime MaxDate
        {
            get => _maxDate;
            set => SetProperty(ref _maxDate, value);
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public MechanicHistoryFilterViewModel(
            List<MechanicHistoryDto> histories,
            TaskCompletionSource<MechanicFilter> completionSource,
            MechanicFilter preSelectedFilter = null)
        {
            _completionSource = completionSource;

            Drivers = new ObservableCollection<PickerItem<int?>>();
            Cars = new ObservableCollection<PickerItem<int?>>();
            Statuses = new ObservableCollection<PickerItem<ReviewStatus?>>();
            SortOptions = new ObservableCollection<PickerItem<string>>();

            ApplyCommand = new Command(OnApply);
            ResetCommand = new Command(OnReset);

            SetupFilterValues(histories);
            SetupPreSelectedFilterValues(preSelectedFilter);
        }

        private void OnApply()
        {
            var filter = new MechanicFilter
            {
                DriverId = SelectedDriver.Value,
                CarId = SelectedCar.Value,
                Status = SelectedStatus.Value,
                SortBy = SelectedSort.Value,
                StartDate = StartDate,
                EndDate = EndDate,
            };

            _completionSource.SetResult(filter);
        }

        private void OnReset()
        {
            SetDefaultSelectedValues();
            // OnApply();
        }

        private void SetupFilterValues(List<MechanicHistoryDto> histories)
        {
            var drivers = histories.Select(x => new PickerItem<int?>(x.DriverId, x.DriverName)).ToList();
            drivers.Insert(0, new PickerItem<int?>(null, "Barcha haydovchilar"));

            var cars = histories.Select(x => new PickerItem<int?>(x.CarId, x.CarDetails)).ToList();
            cars.Insert(0, new PickerItem<int?>(null, "Barcha avtomobillar"));

            var statuses = GetStatusOptions();
            var sortOptions = GetSortOptions();
            MinDate = histories.Min(x => x.Date);
            MaxDate = histories.Max(x => x.Date);

            foreach (var driver in drivers)
            {
                Drivers.Add(driver);
            }

            foreach (var car in cars)
            {
                Cars.Add(car);
            }

            foreach (var status in statuses)
            {
                Statuses.Add(status);
            }

            foreach (var sortOption in sortOptions)
            {
                SortOptions.Add(sortOption);
            }
        }

        private void SetupPreSelectedFilterValues(MechanicFilter filter = null)
        {
            if (filter == null)
            {
                SetDefaultSelectedValues();
                return;
            }

            SelectedDriver = filter.DriverId.HasValue
                ? Drivers.First(x => x.Value == filter.DriverId)
                : Drivers.First(x => x.Value == null);
            SelectedCar = filter.CarId.HasValue
                ? Cars.First(x => x.Value == filter.CarId)
                : Cars.First(x => x.Value == null);
            SelectedStatus = filter.Status.HasValue
                ? Statuses.First(x => x.Value == filter.Status)
                : Statuses.First(x => x.Value == null);
            SelectedSort = SortOptions.First(x => x.Value == filter.SortBy);
            StartDate = filter.StartDate;
            EndDate = filter.EndDate;
        }

        private void SetDefaultSelectedValues()
        {
            SelectedDriver = Drivers.First(x => x.Value == null);
            SelectedCar = Cars.First(x => x.Value == null);
            SelectedStatus = Statuses.First(x => x.Value == null);
            SelectedSort = SortOptions.First(x => x.Value.Equals("date_desc"));
            StartDate = MinDate;
            EndDate = MaxDate;
        }

        private static List<PickerItem<ReviewStatus?>> GetStatusOptions() =>
            new List<PickerItem<ReviewStatus?>>
            {
                new PickerItem<ReviewStatus?>(null, "Barchasi"),
                new PickerItem<ReviewStatus?>(ReviewStatus.Approved, "Tasdiqlangan"),
                new PickerItem<ReviewStatus?>(ReviewStatus.RejectedByReviewer, "Rad etilgan"),
                new PickerItem<ReviewStatus?>(ReviewStatus.RejectedByDriver, "Haydovchi rad qilgan"),
            };

        private static List<PickerItem<string>> GetSortOptions() =>
            new List<PickerItem<string>>()
            {
                new PickerItem<string>("date_desc", "Tekshiruv sanasi (tushish)"),
                new PickerItem<string>("date_asc", "Tekshiruv sanasi (ko'tarilish)"),
                new PickerItem<string>("name_desc", "Haydovchilarni ismi (tushish)"),
                new PickerItem<string>("name_asc", "Haydovchilarni ismi (ko'tarilish)"),
            };
    }
}
