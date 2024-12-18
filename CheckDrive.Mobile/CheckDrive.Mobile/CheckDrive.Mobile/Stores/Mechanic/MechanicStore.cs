using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Mechanic;
using CheckDrive.Mobile.Models.Mechanic.Acceptance;
using CheckDrive.Mobile.Models.Mechanic.Handover;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Stores.Account;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Stores.Mechanic
{
    public class MechanicStore : IMechanicStore
    {
        private readonly ApiClient _client;
        private readonly IAccountStore _accountStore;

        public MechanicStore()
        {
            _client = DependencyService.Get<ApiClient>();
            _accountStore = DependencyService.Get<IAccountStore>();
        }

        public async Task CreateReviewAsync(MechanicHandoverRequest request)
        {
            var response = await _client.PostAsync($"reviews/mechanics/{request.MechanicId}/handover", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateReviewAsync(MechanicAcceptanceRequest request)
        {
            var response = await _client.PostAsync($"reviews/mechanics/{request.MechanicId}/acceptance", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<DriverDto>> GetDriversForAcceptanceReviewAsync()
        {
            var drivers = await _client.GetAsync<List<DriverDto>>($"drivers?=Stage={CheckPointStage.OperatorReview}");

            return drivers;
        }

        public async Task<List<DriverDto>> GetDriversForHandoverReviewAsync()
        {
            var drivers = await _client.GetAsync<List<DriverDto>>($"drivers?Stage={CheckPointStage.DoctorReview}");

            return drivers;
        }

        public async Task<List<MechanicHistoryDto>> GetHistoriesAsync()
        {
            var mechanicId = await _accountStore.GetUserIdAsync();
            var histories = await _client.GetAsync<List<MechanicHistoryDto>>($"reviews/histories/mechanics/{mechanicId}");

            return histories;
        }
    }
}
