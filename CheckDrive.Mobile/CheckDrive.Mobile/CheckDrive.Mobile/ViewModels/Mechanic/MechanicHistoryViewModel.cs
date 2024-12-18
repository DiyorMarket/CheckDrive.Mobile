using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Mechanic;
using CheckDrive.Mobile.Stores.Car;
using CheckDrive.Mobile.Stores.Mechanic;
using CheckDrive.Mobile.ViewModels.Mechanic.Popups;
using CheckDrive.Mobile.Views.Mechanic.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Mechanic
{
    public class MechanicHistoryViewModel : BaseViewModel
    {
        private readonly IMechanicStore _mechanicStore;
        private readonly ICarStore _carStore;

        private readonly List<MechanicHistoryDto> _allHistory;
        private readonly List<CarDto> _cars;
        public ObservableCollection<Grouping<DateTime, MechanicHistoryDto>> FilteredHistories { get; }

        public Command<string> SearchCommand { get; }
        public Command FilterCommand { get; }
        public Command RefreshCommand { get; }

        public MechanicFilter Filters { get; set; }

        public MechanicHistoryViewModel()
        {
            _mechanicStore = DependencyService.Get<IMechanicStore>();
            _carStore = DependencyService.Get<ICarStore>();

            _allHistory = new List<MechanicHistoryDto>();
            _cars = new List<CarDto>();
            FilteredHistories = new ObservableCollection<Grouping<DateTime, MechanicHistoryDto>>();

            SearchCommand = new Command<string>(OnSearch);
            FilterCommand = new Command(async () => await OnFilterAsync());
            RefreshCommand = new Command(async () => await LoadDataAsync());
        }

        public async Task LoadDataAsync()
        {
            IsBusy = true;

            try
            {
                var carsTask = _carStore.GetAvailableCarsAsync();
                var historiesTask = _mechanicStore.GetHistoriesAsync();

                await Task.WhenAll(carsTask, historiesTask);

                _cars.Clear();
                _cars.AddRange(carsTask.Result);
                _allHistory.Clear();
                _allHistory.AddRange(historiesTask.Result);

                var sortedHistories = historiesTask.Result.OrderByDescending(x => x.Date.Date);
                UpdateHistories(sortedHistories);
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

        private void OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            var filteredHistory = string.IsNullOrWhiteSpace(searchText)
                ? _allHistory
                : _allHistory.Where(x => x.DriverName.ToLower().Contains(searchText));

            var sortedHistory = filteredHistory.OrderByDescending(x => x.Date.Date).ToList();
            UpdateHistories(sortedHistory);
        }

        private async Task OnFilterAsync()
        {
            var completionSource = new TaskCompletionSource<MechanicFilter>();
            var options = new MechanicFilterOptions(_allHistory, _cars);
            var popup = new MechanicFiltersPopup()
            {
                BindingContext = new MechanicFiltersViewModel(options, Filters, completionSource)
            };

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);

            var result = await completionSource.Task;
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();

            if (result != null)
            {
                Filters = result;
                ApplyFilters(result);
            }
        }

        private void ApplyFilters(MechanicFilter filter)
        {
            if (filter == null)
            {
                return;
            }

            var query = _allHistory.AsQueryable();

            if (filter.DriverId.HasValue)
            {
                query = query.Where(x => x.DriverId == filter.DriverId);
            }

            if (filter.CarId.HasValue)
            {
                query = query.Where(x => x.CarId == filter.CarId);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(x => x.Status == filter.Status);
            }

            query = query.Where(x => x.Date.Date >= filter.StartDate.Date);
            query = query.Where(x => x.Date.Date <= filter.EndDate.Date);
            query = ApplySort(filter.SortBy, query);

            UpdateHistories(query);
        }

        private void UpdateHistories(IEnumerable<MechanicHistoryDto> histories)
        {
            var groupedHistories = histories
                .GroupBy(x => x.Date.Date)
                .Select(x => new Grouping<DateTime, MechanicHistoryDto>(x.Key, x))
                .ToList();

            FilteredHistories.Clear();
            foreach (var history in groupedHistories)
            {
                FilteredHistories.Add(history);
            }
        }

        private static IQueryable<MechanicHistoryDto> ApplySort(string sortBy, IQueryable<MechanicHistoryDto> query)
        {
            switch (sortBy.ToLower().Trim())
            {
                case "date_asc":
                    return query.OrderBy(x => x.Date.Date);
                case "driver_asc":
                    return query.OrderBy(x => x.DriverName);
                case "driver_desc":
                    return query.OrderByDescending(x => x.DriverName);
                default:
                    return query.OrderByDescending(x => x.Date.Date);
            }
        }
    }
}
