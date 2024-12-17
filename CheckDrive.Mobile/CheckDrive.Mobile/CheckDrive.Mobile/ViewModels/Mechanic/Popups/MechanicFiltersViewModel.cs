using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Mechanic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Mechanic.Popups
{
    public class MechanicFiltersViewModel : BaseViewModel
    {
        private readonly TaskCompletionSource<MechanicFilter> _completionSource;

        public List<PickerItem<int?>> Drivers { get; }
        public List<PickerItem<int?>> Cars { get; }
        public List<PickerItem<ReviewStatus?>> Statuses { get; }
        public List<PickerItem<string>> SortOptions { get; }

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

        public MechanicFiltersViewModel(
            MechanicFilterOptions options,
            MechanicFilter preSelectedFilters,
            TaskCompletionSource<MechanicFilter> completionSource)
        {
            _completionSource = completionSource;

            Drivers = new List<PickerItem<int?>>();
            Cars = new List<PickerItem<int?>>();
            Statuses = new List<PickerItem<ReviewStatus?>>();
            SortOptions = new List<PickerItem<string>>();

            ApplyCommand = new Command(OnApply);
            ResetCommand = new Command(OnReset);

            SetupFilterValues(options);
            SetupPreSelectedFilterValues(preSelectedFilters);
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
            var defaultFilters = MechanicFilter.GetDefaultFilters(MinDate, MaxDate);
            _completionSource.SetResult(defaultFilters);
        }

        private void SetupFilterValues(MechanicFilterOptions options)
        {
            Drivers.AddRange(options.Drivers);
            Cars.AddRange(options.Cars);
            Statuses.AddRange(MechanicFilterOptions.Statuses);
            SortOptions.AddRange(MechanicFilterOptions.SortOptions);
            MinDate = options.MinDate;
            MaxDate = options.MaxDate;
        }

        private void SetupPreSelectedFilterValues(MechanicFilter filter = null)
        {
            if (filter == null)
            {
                SetDefaultSelectedValues();
                return;
            }

            SelectedDriver = filter.DriverId.HasValue
                ? Drivers.Find(x => x.Value == filter.DriverId)
                : Drivers.Find(x => x.Value == null);
            SelectedCar = filter.CarId.HasValue
                ? Cars.Find(x => x.Value == filter.CarId)
                : Cars.Find(x => x.Value == null);
            SelectedStatus = filter.Status.HasValue
                ? Statuses.Find(x => x.Value == filter.Status)
                : Statuses.Find(x => x.Value == null);
            SelectedSort = SortOptions.Find(x => x.Value == filter.SortBy);
            StartDate = filter.StartDate;
            EndDate = filter.EndDate;
        }

        private void SetDefaultSelectedValues()
        {
            SelectedDriver = Drivers.Find(x => x.Value == null);
            SelectedCar = Cars.Find(x => x.Value == null);
            SelectedStatus = Statuses.Find(x => x.Value == null);
            SelectedSort = SortOptions.Find(x => x.Value.Equals("date_desc"));
            StartDate = MinDate;
            EndDate = MaxDate;
        }
    }
}
