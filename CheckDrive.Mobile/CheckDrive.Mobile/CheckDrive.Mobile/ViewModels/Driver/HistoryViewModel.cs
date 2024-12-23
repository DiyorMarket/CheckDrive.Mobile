using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Stores.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels.Driver
{
    public class HistoryViewModel : BaseViewModel
    {
        private readonly IDriverStore _driverStore;
        private readonly List<DriverHistory> _allHistory;

        private string _search;
        public string Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }

        public ObservableCollection<Grouping<DateTime, DriverHistory>> Histories { get; private set; }

        public Command RefreshCommand { get; }
        public Command<string> SearchCommmand { get; }

        public HistoryViewModel()
        {
            _driverStore = DependencyService.Get<IDriverStore>();
            _allHistory = new List<DriverHistory>();

            RefreshCommand = new Command(async () => await LoadHistories());
            SearchCommmand = new Command<string>(OnSearch);

            Histories = new ObservableCollection<Grouping<DateTime, DriverHistory>>();
        }

        public async Task LoadHistories()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var histories = await _driverStore.GetHistoriesAsync();

                _allHistory.Clear();
                _allHistory.AddRange(histories);

                UpdateHistories(histories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnSearch(string search)
        {
            search = search.ToLower().Trim();

            var filteredHistories = string.IsNullOrEmpty(search)
                ? _allHistory
                : _allHistory.Where(x => x.CarName.ToLower().Contains(search));

            UpdateHistories(filteredHistories);
        }

        private void UpdateHistories(IEnumerable<DriverHistory> histories)
        {
            var groupedHistory = histories
                .GroupBy(x => x.StartDate.Date)
                .Select(x => new Grouping<DateTime, DriverHistory>(x.Key, x));

            Histories.Clear();
            foreach (var history in groupedHistory)
            {
                Histories.Add(history);
            }
        }
    }
}
