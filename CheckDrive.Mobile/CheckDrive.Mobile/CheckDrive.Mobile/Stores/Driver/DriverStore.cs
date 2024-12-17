using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Stores.Account;

namespace CheckDrive.Mobile.Stores.Driver
{
    public class DriverStore : IDriverStore
    {
        private readonly ApiClient _client;
        private readonly IAccountStore _accountStore;

        public DriverStore()
        {
            _client = DependencyService.Get<ApiClient>();
            _accountStore = DependencyService.Get<IAccountStore>();
        }

        public async Task<List<DriverDto>> GetDriversAsync(DriverStatus status = DriverStatus.Available)
        {
            var drivers = await _client.GetAsync<List<DriverDto>>($"drivers?Status={status}");

            return drivers;
        }

        public async Task<CheckPointDto> GetCurrentCheckPointAsync()
        {
            var driverId = await _accountStore.GetUserIdAsync();
            var checkPoint = await _client.GetAsync<CheckPointDto>($"checkPoints/drivers/{driverId}/current");

            return checkPoint;
        }

        public async Task SendReviewConfirmationAsync(ReviewConfirmationRequest request)
        {
            await _client.PostAsync("drivers/reviews", request);
        }

        public async Task<List<CheckPointHistoryDto>> GetHistoriesAsync()
        {
            var userId = await _accountStore.GetUserIdAsync();
            var checkPoints = await _client.GetAsync<List<CheckPointHistoryDto>>($"reviews/histories/drivers/{userId}");

            return checkPoints;
        }
    }
}
