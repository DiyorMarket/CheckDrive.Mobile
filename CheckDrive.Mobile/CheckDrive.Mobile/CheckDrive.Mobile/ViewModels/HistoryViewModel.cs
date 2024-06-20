using CheckDrive.ApiContracts.Driver;
using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Services;
using CheckDrive.Web.Stores.DoctorReviews;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.OperatorReviews;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        private readonly IDoctorReviewDataStore _doctorReviewDataStore;
        private readonly IMechanicHandoverDataStore _mechanicHandoverDataStore;
        private readonly IOperatorReviewDataStore _operatorReviewDataStore;
        private readonly IMechanicAcceptanceDataStore _mechanicAcceptanceDataStore;
        private readonly DriverDto _driver = DataService.GetAccount();

        public ObservableCollection<History> Reviews { get; private set; }

        public HistoryViewModel(IDoctorReviewDataStore doctorReviewDataStore, IMechanicHandoverDataStore mechanicHandoverDataStore, IOperatorReviewDataStore operatorReviewDataStore, IMechanicAcceptanceDataStore mechanicAcceptanceDataStore)
        {
            _doctorReviewDataStore = doctorReviewDataStore;
            _mechanicHandoverDataStore = mechanicHandoverDataStore;
            _operatorReviewDataStore = operatorReviewDataStore;
            _mechanicAcceptanceDataStore = mechanicAcceptanceDataStore;

            Reviews = new ObservableCollection<History>();

            LoadViewPage();
        }

        public async void LoadViewPage()
        {

            IsBusy = true;
            await Task.Run(() => {
                GetDispatcherReviews();
            });
            IsBusy = false;
        }
        public async void GetDispatcherReviews()
        {
            Reviews.Clear();
            IsBusy = true;

            var doctorItemsResponse = await _doctorReviewDataStore.GetDoctorReviewsByDriverIdAsync(_driver.Id);
            var doctorItems = doctorItemsResponse.Data;
            var mechanicHandoverItemsResponse = await _mechanicHandoverDataStore.GetMechanicHandoversByDriverIdAsync(2);
            var mechanicHandoverItems = mechanicHandoverItemsResponse.Data;
            var operatorItemsResponse = await _operatorReviewDataStore.GetOperatorReviewsByDriverIdAsync(2);
            var operatorItems = operatorItemsResponse.Data;
            var mechanicAcceptenceResponse = await _mechanicAcceptanceDataStore.GetMechanicAcceptancesByDriverIdAsync(2);
            var mechanicAcceptence = mechanicAcceptenceResponse.Data;

            int itemCount = Math.Min(Math.Min(doctorItems.Count(), mechanicHandoverItems.Count()), Math.Min(operatorItems.Count(), mechanicAcceptence.Count()));

            for (int i = 0; i < itemCount; i++)
            {
                var historyItem = new History
                {
                    Date = doctorItems.ToList()[i].Date,
                    IsHealthy = doctorItems.ToList()[i].IsHealthy,
                    IsHanded = mechanicHandoverItems.ToList()[i].IsHanded,
                    IsGiven = operatorItems.ToList()[i].IsGiven,
                    IsAccepted = mechanicAcceptence.ToList()[i].IsAccepted
                };

                Reviews.Add(historyItem);
            }

            IsBusy = false;
        }
    }
}
