using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Operator;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Operator.Popups
{
    public class OperatorFiltersViewModel : BaseViewModel
    {
        private readonly TaskCompletionSource<OperatorFilter> _completionSource;

        public List<PickerItem<int?>> Drivers { get; }
        public List<PickerItem<int?>> OilMarks { get; }
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

        private PickerItem<int?> _selectedOilMark;
        public PickerItem<int?> SelectedOilMark
        {
            get => _selectedOilMark;
            set => SetProperty(ref _selectedOilMark, value);
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

        public OperatorFiltersViewModel(
            OperatorFilterOptions options,
            OperatorFilter preSelectedFilters,
            TaskCompletionSource<OperatorFilter> completionSource)
        {
            _completionSource = completionSource;

            Drivers = new List<PickerItem<int?>>();
            OilMarks = new List<PickerItem<int?>>();
            Statuses = new List<PickerItem<ReviewStatus?>>();
            SortOptions = new List<PickerItem<string>>();

            ApplyCommand = new Command(async () => await OnApplyAsync());
            ResetCommand = new Command(async () => await OnResetAsync());

            SetupFilterValues(options);
            SetupPreSelectedFilterValues(preSelectedFilters);
        }

        private async Task OnApplyAsync()
        {
            var filter = new OperatorFilter
            {
                DriverId = SelectedDriver.Value,
                OilMarkId = SelectedOilMark.Value,
                Status = SelectedStatus.Value,
                SortBy = SelectedSort.Value,
                StartDate = StartDate,
                EndDate = EndDate,
            };

            await PopupNavigation.Instance.PopAsync();

            _completionSource.SetResult(filter);
        }

        private async Task OnResetAsync()
        {
            var defaultFilters = OperatorFilter.GetDefaultFilters(MinDate, MaxDate);

            await PopupNavigation.Instance.PopAsync();

            _completionSource.SetResult(defaultFilters);
        }

        private void SetupFilterValues(OperatorFilterOptions options)
        {
            Drivers.AddRange(options.Drivers);
            OilMarks.AddRange(options.OilMarks);
            Statuses.AddRange(OperatorFilterOptions.Statuses);
            SortOptions.AddRange(OperatorFilterOptions.SortOptions);
            MinDate = options.MinDate;
            MaxDate = options.MaxDate;
        }

        private void SetupPreSelectedFilterValues(OperatorFilter filter = null)
        {
            if (filter == null)
            {
                SetDefaultSelectedValues();
                return;
            }

            SelectedDriver = filter.DriverId.HasValue
                ? Drivers.Find(x => x.Value == filter.DriverId)
                : Drivers.Find(x => x.Value == null);
            SelectedOilMark = filter.OilMarkId.HasValue
                ? OilMarks.Find(x => x.Value == filter.OilMarkId)
                : OilMarks.Find(x => x.Value == null);
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
            SelectedOilMark = OilMarks.Find(x => x.Value == null);
            SelectedStatus = Statuses.Find(x => x.Value == null);
            SelectedSort = SortOptions.Find(x => x.Value.Equals("date_desc"));
            StartDate = MinDate;
            EndDate = MaxDate;
        }
    }
}
