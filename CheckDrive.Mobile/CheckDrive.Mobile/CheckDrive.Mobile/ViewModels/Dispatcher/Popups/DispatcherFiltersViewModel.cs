using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Dispatcher;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Dispatcher.Popups
{
    public class DispatcherFiltersViewModel : BaseViewModel
    {
        private readonly TaskCompletionSource<DispatcherFilter> _completionSource;

        public List<PickerItem<int?>> Drivers { get; }
        public List<PickerItem<string>> SortOptions { get; }

        private PickerItem<int?> _selectedDriver;
        public PickerItem<int?> SelectedDriver
        {
            get => _selectedDriver;
            set => SetProperty(ref _selectedDriver, value);
        }

        private PickerItem<string> _selectedSortOption;
        public PickerItem<string> SelectedSortOption
        {
            get => _selectedSortOption;
            set => SetProperty(ref _selectedSortOption, value);
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

        public Command ApplyCommand { get; }
        public Command ResetCommand { get; }

        public DispatcherFiltersViewModel(
            DispatcherFilterOptions options,
            DispatcherFilter preSelectedFilters,
            TaskCompletionSource<DispatcherFilter> completionSource)
        {
            _completionSource = completionSource;

            Drivers = new List<PickerItem<int?>>();
            SortOptions = new List<PickerItem<string>>();

            ApplyCommand = new Command(async () => await OnApplyAsync());
            ResetCommand = new Command(async () => await OnResetAsync());

            SetupFilterValues(options);
            SetupPreSelectedFilters(preSelectedFilters);
        }

        private async Task OnApplyAsync()
        {
            await PopupNavigation.Instance.PopAsync();

            var filter = new DispatcherFilter(
                selectedDriverId: SelectedDriver.Value,
                startDate: StartDate,
                endDate: EndDate,
                sortBy: SelectedSortOption.Value);

            _completionSource.SetResult(filter);
        }

        private async Task OnResetAsync()
        {
            await PopupNavigation.Instance.PopAsync();

            var defaultFilter = DispatcherFilter.GetDefaultFilter(MinDate, EndDate);
            _completionSource.SetResult(defaultFilter);
        }

        private void SetupFilterValues(DispatcherFilterOptions options)
        {
            Drivers.AddRange(options.Drivers);
            SortOptions.AddRange(DispatcherFilterOptions.SortOptions);
            MinDate = options.MinDate;
            MaxDate = options.MaxDate;
        }

        private void SetupPreSelectedFilters(DispatcherFilter preSelectedFilters)
        {
            if (preSelectedFilters is null)
            {
                SetupDefaultSelectedValues();
                return;
            }

            var selectedDriver = Drivers.Find(x => x.Value == preSelectedFilters.SelectedDriverId);
            SelectedDriver = selectedDriver is null ? Drivers.Find(x => x.Value == null) : selectedDriver;
            SelectedSortOption = SortOptions.Find(x => x.Value == preSelectedFilters.SortBy);
            StartDate = preSelectedFilters.StartDate;
            EndDate = preSelectedFilters.EndDate;
        }

        private void SetupDefaultSelectedValues()
        {
            SelectedDriver = Drivers.Find(x => x.Value == null);
            SelectedSortOption = SortOptions.Find(x => x.Value == "date_desc");
            StartDate = MinDate;
            EndDate = MaxDate;
        }
    }
}
