using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Doctor
{
    public class DoctorFiltersViewModel : BaseViewModel
    {
        private readonly TaskCompletionSource<DoctorFilter> _completionSource;

        public ObservableCollection<PickerItem<int?>> Drivers { get; }
        public ObservableCollection<PickerItem<ReviewStatus?>> Statuses { get; }
        public ObservableCollection<PickerItem<string>> SortOptions { get; }

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

        public DoctorFiltersViewModel(
            List<DoctorHistory> histories,
            TaskCompletionSource<DoctorFilter> completionSource,
            DoctorFilter preSelectedFilter = null)
        {
            _completionSource = completionSource;

            ApplyCommand = new Command(OnApply);
            ResetCommand = new Command(OnReset);

            Drivers = new ObservableCollection<PickerItem<int?>>();
            Statuses = new ObservableCollection<PickerItem<ReviewStatus?>>();
            SortOptions = new ObservableCollection<PickerItem<string>>();

            SetupFilterValues(histories);
            SetupPreSelectedValues(preSelectedFilter);
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
            var defaultFilters = GetDefaultFilters();
            _completionSource.SetResult(defaultFilters);
        }

        private void SetupFilterValues(List<DoctorHistory> histories)
        {
            var drivers = histories.Select(x => new PickerItem<int?>(x.DriverId, x.DriverName)).ToList();
            drivers.Insert(0, new PickerItem<int?>(null, "Barcha haydovchilar"));

            var statuses = new List<PickerItem<ReviewStatus?>>
            {
                new PickerItem<ReviewStatus?>(null, "Barchasi"),
                new PickerItem<ReviewStatus?>(ReviewStatus.Approved, "Tasdiqlangan"),
                new PickerItem<ReviewStatus?>(ReviewStatus.RejectedByReviewer, "Rad etilgan"),
            };

            var sortOptions = new List<PickerItem<string>>()
            {
                new PickerItem<string>("date_desc", "Tekshiruv sanasi (tushish)"),
                new PickerItem<string>("date_asc", "Tekshiruv sanasi (ko'tarilish)"),
                new PickerItem<string>("name_desc", "Haydovchilarni ismi (tushish)"),
                new PickerItem<string>("name_asc", "Haydovchilarni ismi (ko'tarilish)"),
            };

            foreach (var driver in drivers)
            {
                Drivers.Add(driver);
            }

            foreach (var status in statuses)
            {
                Statuses.Add(status);
            }

            foreach (var sortOption in sortOptions)
            {
                SortOptions.Add(sortOption);
            }

            MinDate = histories.Any()
                ? histories.Min(x => x.Date)
                : DateTime.Today;
            MaxDate = histories.Any()
                ? histories.Max(x => x.Date)
                : DateTime.Today;
        }

        private void SetupPreSelectedValues(DoctorFilter preSelectedFilter = null)
        {
            if (preSelectedFilter is null)
            {
                SetupDefaultSelectedValues();
                return;
            }

            var selectedDriver = Drivers.FirstOrDefault(x => x.Value == preSelectedFilter.SelectedDriverId);
            SelectedDriver = selectedDriver is null
                ? Drivers.First(x => x.Value == null)
                : selectedDriver;
            SelectedStatus = Statuses.First(x => x.Value == preSelectedFilter.SelectedStatus);
            SelectedSort = SortOptions.First(x => x.Value == preSelectedFilter.SortBy);
            StartDate = preSelectedFilter.StartDate;
            EndDate = preSelectedFilter.EndDate;
        }

        private void SetupDefaultSelectedValues()
        {
            SelectedDriver = Drivers.First(x => x.Value == null);
            SelectedStatus = Statuses.First(x => x.Value == null);
            SelectedSort = SortOptions.First(x => x.Value == "date_desc");
            StartDate = MinDate;
            EndDate = MaxDate;
        }

        private DoctorFilter GetDefaultFilters()
        {
            var filter = new DoctorFilter()
            {
                SelectedDriverId = null,
                SelectedStatus = null,
                SortBy = "date_desc",
                StartDate = StartDate,
                EndDate = EndDate,
            };

            return filter;
        }
    }
}
