using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Doctor.Popups
{
    public class DoctorFiltersViewModel : BaseViewModel
    {
        private readonly TaskCompletionSource<DoctorFilter> _completionSource;

        public List<PickerItem<int?>> Drivers { get; }
        public List<PickerItem<ReviewStatus?>> Statuses { get; }
        public List<PickerItem<string>> SortOptions { get; }

        public Command ApplyCommand { get; }
        public Command ResetCommand { get; }

        private PickerItem<int?> _selectedDriver;
        public PickerItem<int?> SelectedDriver
        {
            get => _selectedDriver;
            set => SetProperty(ref _selectedDriver, value);
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

        public DoctorFiltersViewModel(DoctorFilterOptions options, DoctorFilter preSelectedFilters, TaskCompletionSource<DoctorFilter> completionSource)
        {
            _completionSource = completionSource;

            ApplyCommand = new Command(OnApply);
            ResetCommand = new Command(OnReset);

            Drivers = new List<PickerItem<int?>>();
            Statuses = new List<PickerItem<ReviewStatus?>>();
            SortOptions = new List<PickerItem<string>>();

            SetupFilterValues(options);
            SetupPreSelectedValues(preSelectedFilters);
        }

        private void OnApply()
        {
            var filter = new DoctorFilter()
            {
                SelectedDriverId = SelectedDriver.Value,
                SelectedStatus = SelectedStatus.Value,
                SortBy = SelectedSort.Value,
                StartDate = StartDate,
                EndDate = EndDate,
            };

            _completionSource.SetResult(filter);
        }

        private void OnReset()
        {
            var defaultFilters = DoctorFilter.GetDefaultFilter(MinDate, MaxDate);
            _completionSource.SetResult(defaultFilters);
        }

        private void SetupFilterValues(DoctorFilterOptions options)
        {
            Drivers.AddRange(options.Drivers);
            Statuses.AddRange(DoctorFilterOptions.Statuses);
            SortOptions.AddRange(DoctorFilterOptions.SortOptions);
            MinDate = options.MinDate;
            MaxDate = options.MaxDate;
        }

        private void SetupPreSelectedValues(DoctorFilter preSelectedFilters = null)
        {
            if (preSelectedFilters is null)
            {
                SetupDefaultSelectedValues();
                return;
            }

            var selectedDriver = Drivers.Find(x => x.Value == preSelectedFilters.SelectedDriverId);
            SelectedDriver = selectedDriver is null ? Drivers.First(x => x.Value == null) : selectedDriver;
            SelectedStatus = Statuses.First(x => x.Value == preSelectedFilters.SelectedStatus);
            SelectedSort = SortOptions.First(x => x.Value == preSelectedFilters.SortBy);
            StartDate = preSelectedFilters.StartDate;
            EndDate = preSelectedFilters.EndDate;
        }

        private void SetupDefaultSelectedValues()
        {
            SelectedDriver = Drivers.First(x => x.Value == null);
            SelectedStatus = Statuses.First(x => x.Value == null);
            SelectedSort = SortOptions.First(x => x.Value == "date_desc");
            StartDate = MinDate;
            EndDate = MaxDate;
        }
    }
}
