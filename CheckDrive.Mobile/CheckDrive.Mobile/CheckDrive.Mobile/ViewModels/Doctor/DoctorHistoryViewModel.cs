using System.Collections.ObjectModel;
using System;
using Xamarin.CommunityToolkit.ObjectModel;
using CheckDrive.Mobile.Models.Doctor;
using System.Threading.Tasks;
using CheckDrive.Mobile.Stores.Doctor;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace CheckDrive.Mobile.ViewModels.Doctor
{
    public class DoctorHistoryViewModel : BaseViewModel
    {
        private readonly IDoctorStore _doctorStore;

        private readonly List<DoctorHistory> _histories;
        public ObservableCollection<Grouping<DateTime, DoctorHistory>> Histories { get; set; }

        public Command RefreshCommand { get; }
        public Command<string> SearchCommand { get; }

        public DoctorHistoryViewModel()
        {
            _doctorStore = DependencyService.Get<IDoctorStore>();

            _histories = new List<DoctorHistory>();
            Histories = new ObservableCollection<Grouping<DateTime, DoctorHistory>>();

            RefreshCommand = new Command(async () => await LoadHistoryAsync());
            SearchCommand = new Command<string>(OnSearch);
        }

        public async Task LoadHistoryAsync()
        {
            IsRefreshing = true;

            try
            {
                var reviewHistories = await _doctorStore.GetReviewHistoryAsync();
                var groupedHistory = reviewHistories
                    .OrderByDescending(x => x.Date)
                    .GroupBy(d => d.Date.Date)
                    .Select(g => new Grouping<DateTime, DoctorHistory>(g.Key, g))
                    .ToList();

                _histories.Clear();
                _histories.AddRange(reviewHistories);

                Histories.Clear();

                foreach (var history in groupedHistory)
                {
                    Histories.Add(history);
                }
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Tarixni yuklashda xato ro'y berdi. Iltimos, qayta urunib ko'ring yoki texnik yordam bilan bog'laning.", ex.Message);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private void OnSearch(string searchText)
        {
            List<DoctorHistory> histories = string.IsNullOrWhiteSpace(searchText)
                ? _histories
                : _histories.Where(x => x.DriverName.ToLower().Contains(searchText.ToLower())).ToList();

            var groupedHistory = histories
                        .OrderByDescending(x => x.Date)
                        .GroupBy(d => d.Date.Date)
                        .Select(g => new Grouping<DateTime, DoctorHistory>(g.Key, g))
                        .ToList();

            Histories.Clear();
            foreach (var history in groupedHistory)
            {
                Histories.Add(history);
            }
        }
    }
}
