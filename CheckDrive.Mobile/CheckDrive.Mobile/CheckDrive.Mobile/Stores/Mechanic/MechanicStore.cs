using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Mechanic;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Stores.Account;
using System;
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

        public async Task CreateReviewAsync(MechanicHandoverReview review)
        {
            var userId = await _accountStore.GetUserIdAsync();
            var response = await _client.PostAsync<MechanicHandoverReview>($"reviews/mechanics/{userId}/handover", review);
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateReviewAsync(MechanicAcceptanceReview review)
        {
            var response = await _client.PostAsync($"reviews/mechanics/{review.ReviewerId}/acceptance", review);
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

        public Task<List<MechanicHistoryDto>> GetHistoriesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
