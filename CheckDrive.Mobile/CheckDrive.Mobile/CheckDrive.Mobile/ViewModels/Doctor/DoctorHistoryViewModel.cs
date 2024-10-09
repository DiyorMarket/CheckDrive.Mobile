using System.Collections.ObjectModel;
using System;
using Xamarin.CommunityToolkit.ObjectModel;
using CheckDrive.Mobile.Models.Doctor;
using System.Threading.Tasks;
using CheckDrive.Mobile.Stores.Doctor;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using CheckDrive.Mobile.Views.Doctor;
using CheckDrive.Mobile.Models.Enums;

namespace CheckDrive.Mobile.ViewModels.Doctor
{
    public class DoctorHistoryViewModel : BaseViewModel
    {
        private readonly IDoctorStore _doctorStore;

        private readonly List<DoctorHistory> _histories;
        public ObservableCollection<Grouping<DateTime, DoctorHistory>> Histories { get; set; }

        public Command RefreshCommand { get; }
        public Command<string> SearchCommand { get; }
        public Command FilterCommand { get; }

        private DoctorFilter _filters = null;
        public DoctorFilter Filters
        {
            get => _filters;
            set => SetProperty(ref _filters, value);
        }

        public DoctorHistoryViewModel()
        {
            _doctorStore = DependencyService.Get<IDoctorStore>();

            _histories = new List<DoctorHistory>();
            Histories = new ObservableCollection<Grouping<DateTime, DoctorHistory>>();

            RefreshCommand = new Command(async () => await LoadHistoryAsync());
            SearchCommand = new Command<string>(OnSearch);
            FilterCommand = new Command(async () => await OnFilterAsync());
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

            UpdateHistories(histories);
        }

        private async Task OnFilterAsync()
        {
            var completionSource = new TaskCompletionSource<DoctorFilter>();
            var popup = new DoctorFiltersPopup
            {
                BindingContext = new DoctorFiltersViewModel(_histories, completionSource, Filters)
            };

            try
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup, true);

                var result = await completionSource.Task;
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();

                if (result != null)
                {
                    Filters = result;
                    ApplyFilters(result);
                }
            }
            catch (Exception ex)
            {
                await DisplayErrorAsync("Saralash modalni ochishda muammo ro'y berdi.", ex.Message);
            }
        }

        private void ApplyFilters(DoctorFilter filters)
        {
            var query = _histories.AsQueryable();

            if (filters.SelectedDriverId.HasValue)
            {
                query = query.Where(x => x.DriverId == filters.SelectedDriverId.Value);
            }

            if (filters.SelectedStatus.HasValue)
            {
                query = query.Where(x => filters.SelectedStatus.Value == ReviewStatus.Approved ? x.IsApproved : !x.IsApproved);
            }

            query = query.Where(x => x.Date.Date >= filters.StartDate.Date);
            query = query.Where(x => x.Date.Date <= filters.EndDate.Date);
            query = SortHistory(query, filters.SortBy);

            UpdateHistories(query);
        }

        private void UpdateHistories(IEnumerable<DoctorHistory> histories)
        {
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

        private static IQueryable<DoctorHistory> SortHistory(IQueryable<DoctorHistory> query, string sortBy)
        {
            switch (sortBy.ToLower().Trim())
            {
                case "date_asc":
                    return query.OrderBy(x => x.Date);
                case "name_asc":
                    return query.OrderBy(x => x.DriverName);
                case "name_desc":
                    return query.OrderByDescending(x => x.DriverName);
                default:
                    return query.OrderByDescending(x => x.Date);
            }
        }
    }
}
