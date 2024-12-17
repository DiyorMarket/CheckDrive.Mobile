using CheckDrive.Mobile.Models.Operator;
using CheckDrive.Mobile.Stores.Operator;
using CheckDrive.Mobile.Views.Operator.Popups;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Operator
{
    public class OperatorHistoryViewModel : BaseViewModel
    {
        private readonly IOperatorStore _operatorStore;

        private readonly List<OperatorHistoryDto> _allHistories;
        public ObservableCollection<Grouping<DateTime, OperatorHistoryDto>> FilteredHistories { get; }

        public Command<string> SearchCommand { get; }
        public Command ShowFiltersCommand { get; }
        public Command RefreshCommand { get; }

        public OperatorFilter Filters { get; private set; }
        public string CurrentDate { get; }

        public OperatorHistoryViewModel()
        {
            _operatorStore = DependencyService.Get<IOperatorStore>();

            _allHistories = new List<OperatorHistoryDto>();
            FilteredHistories = new ObservableCollection<Grouping<DateTime, OperatorHistoryDto>>();

            CurrentDate = DateTime.Now.ToString("dd MMMM");

            SearchCommand = new Command<string>(OnSearch);
            ShowFiltersCommand = new Command(async () => await OnShowFiltersAsync());
            RefreshCommand = new Command(async () => await LoadHistoryAsync());
        }

        public async Task LoadHistoryAsync()
        {
            IsBusy = true;

            try
            {
                var histories = await _operatorStore.GetHistoriesAsync();
                var sortedHistory = histories.OrderByDescending(x => x.Date);

                _allHistories.Clear();
                _allHistories.AddRange(sortedHistory);

                UpdateHistories(sortedHistory);
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Tarixni yukalshda kutilmagan xato ro'y berdi.", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();
            var filteredHistories = string.IsNullOrEmpty(searchText)
                ? _allHistories
                : _allHistories.Where(x => x.DriverName.ToLower().Contains(searchText));

            UpdateHistories(filteredHistories);
        }

        private async Task OnShowFiltersAsync()
        {
            try
            {
                var completionSource = new TaskCompletionSource<OperatorFilter>();
                var options = new OperatorFilterOptions(_allHistories);
                var popup = new OperatorFiltersPopup(options, Filters, completionSource);

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
                await DisplayErrorAsync("Taxrirlash shaxobchasini ochishda kutilmagan xato ro'y berdi.", ex.Message);
            }
        }

        private void ApplyFilters(OperatorFilter filter)
        {
            if (filter == null)
            {
                return;
            }

            var query = _allHistories
                .Where(x => x.Date.Date >= filter.StartDate.Date)
                .Where(x => x.Date.Date <= filter.EndDate.Date);

            if (filter.DriverId.HasValue)
            {
                query = query.Where(x => x.DriverId == filter.DriverId);
            }

            if (filter.OilMarkId.HasValue)
            {
                query = query.Where(x => x.OilMarkId == filter.OilMarkId);
            }

            query = ApplySort(filter.SortBy, query);

            UpdateHistories(query);
        }

        private static IEnumerable<OperatorHistoryDto> ApplySort(string sortBy, IEnumerable<OperatorHistoryDto> query)
        {
            switch (sortBy.ToLower().Trim())
            {
                case "date_asc":
                    return query.OrderBy(x => x.Date.Date);
                case "name_asc":
                    return query.OrderBy(x => x.DriverName);
                case "name_desc":
                    return query.OrderByDescending(x => x.DriverName);
                default:
                    return query.OrderByDescending(x => x.Date.Date);
            }
        }

        private void UpdateHistories(IEnumerable<OperatorHistoryDto> histories)
        {
            var groupedHistory = histories
                .GroupBy(x => x.Date.Date)
                .Select(g => new Grouping<DateTime, OperatorHistoryDto>(g.Key, g))
                .ToList();

            FilteredHistories.Clear();

            foreach (var history in groupedHistory)
            {
                FilteredHistories.Add(history);
            }
        }
    }
}
