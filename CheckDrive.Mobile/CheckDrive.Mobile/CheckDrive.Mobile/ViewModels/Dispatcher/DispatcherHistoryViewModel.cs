using CheckDrive.Mobile.Models.Dispatcher;
using CheckDrive.Mobile.Stores.Dispatcher;
using CheckDrive.Mobile.Views.Dispatcher.Popups;
using Rg.Plugins.Popup.Services;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Dispatcher
{
    public class DispatcherHistoryViewModel : BaseViewModel
    {
        private readonly IDispatcherStore _dispatcherStore;
        private readonly List<DispatcherHistory> _allHistory;

        public ObservableCollection<Grouping<DateTime, DispatcherHistory>> FilteredHistory { get; }

        public Command RefreshCommand { get; }
        public Command ShowFiltersCommand { get; }
        public Command<string> SearchCommand { get; }

        private DispatcherFilter _filters;
        public DispatcherFilter Filters
        {
            get => _filters;
            set => SetProperty(ref _filters, value);
        }

        public DispatcherHistoryViewModel()
        {
            _dispatcherStore = DependencyService.Get<IDispatcherStore>();

            _allHistory = new List<DispatcherHistory>();
            FilteredHistory = new ObservableCollection<Grouping<DateTime, DispatcherHistory>>();

            RefreshCommand = new Command(async () => await LoadHistoryAsync());
            ShowFiltersCommand = new Command(async () => await OnShowFiltersAsync());
            SearchCommand = new Command<string>(OnSearch);
        }

        public async Task LoadHistoryAsync()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var history = await _dispatcherStore.GetHistoryAsync();

                _allHistory.Clear();
                _allHistory.AddRange(history);

                UpdateHistory(history);
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Tarixni yuklashda xato ro'y berdi.", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnShowFiltersAsync()
        {
            try
            {
                var completionSource = new TaskCompletionSource<DispatcherFilter>();
                var options = new DispatcherFilterOptions(_allHistory);
                var popup = new DispatcherFilterPopup(options, Filters, completionSource);

                await PopupNavigation.Instance.PushAsync(popup);

                var result = await completionSource.Task;

                if (result != null)
                {
                    Filters = result;
                    ApplyFilters(result);
                }
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Kutilmagan xato ro'y berdi.", ex.Message);
            }
        }

        private void OnSearch(string search)
        {
            var searchText = search.ToLower().Trim();
            var filteredHistory = _allHistory
                .Where(x => x.DriverName.ToLower().Contains(searchText));

            UpdateHistory(filteredHistory);
        }

        private void ApplyFilters(DispatcherFilter filter)
        {
            if (filter is null)
            {
                UpdateHistory(_allHistory);
                return;
            }

            var query = _allHistory.Where(x => x.Date.Date >= filter.StartDate.Date && x.Date.Date <= filter.EndDate.Date);

            if (filter.SelectedDriverId.HasValue)
            {
                query = query.Where(x => x.DriverId == filter.SelectedDriverId.Value);
            }

            query = SortHistory(query, filter.SortBy);

            UpdateHistory(query);
        }

        private void UpdateHistory(IEnumerable<DispatcherHistory> histories)
        {
            var groupedHistory = histories
                .GroupBy(x => x.Date.Date)
                .Select(x => new Grouping<DateTime, DispatcherHistory>(x.Key, x));

            FilteredHistory.Clear();

            foreach (var history in groupedHistory)
            {
                FilteredHistory.Add(history);
            }
        }

        private static IEnumerable<DispatcherHistory> SortHistory(IEnumerable<DispatcherHistory> query, string sortBy)
        {
            switch (sortBy)
            {
                case "name_asc":
                    return query.OrderBy(x => x.DriverName);
                case "name_desc":
                    return query.OrderByDescending(x => x.DriverName);
                case "date_asc":
                    return query.OrderBy(x => x.Date.Date);
                default:
                    return query.OrderByDescending(x => x.Date.Date);
            }
        }
    }
}
