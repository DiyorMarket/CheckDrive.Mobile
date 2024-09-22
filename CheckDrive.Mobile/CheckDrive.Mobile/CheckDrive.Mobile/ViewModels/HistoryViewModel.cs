using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Stores.History;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        private readonly IHistoryStore _historyStore;

        private string _search;
        public string Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }

        public ObservableCollection<CheckPointHistoryDto> Histories { get; private set; }

        public HistoryViewModel()
        {
            _historyStore = DependencyService.Get<IHistoryStore>();

            Histories = new ObservableCollection<CheckPointHistoryDto>();
        }

        public async Task LoadHistories()
        {
            IsBusy = true;
            Histories.Clear();

            try
            {
                var histories = await _historyStore.GetHistoriesAsync();

                foreach (var history in histories)
                {
                    Histories.Add(history);
                }
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
    }
}
