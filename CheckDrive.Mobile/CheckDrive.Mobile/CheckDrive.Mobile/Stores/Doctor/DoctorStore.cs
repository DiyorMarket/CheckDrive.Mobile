using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Stores.Account;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Stores.Doctor
{
    public class DoctorStore : IDoctorStore
    {
        private readonly ApiClient _client;
        private readonly IAccountStore _accountStore;

        public DoctorStore()
        {
            _client = DependencyService.Get<ApiClient>();
            _accountStore = DependencyService.Get<IAccountStore>();
        }

        public async Task<List<DriverDto>> GetDriversAsync()
        {
            var drivers = await _client.GetAsync<List<DriverDto>>("drivers");

            return drivers;
        }

        public async Task<List<DoctorReview>> GetReviews(bool forceRefresh = false)
        {
            var doctorId = await _accountStore.GetUserIdAsync();
            var reviews = await _client.GetAsync<List<DoctorReview>>($"reviews/histories/doctors/{doctorId}");

            return reviews;
        }

        public async Task CreateAsync(DoctorReviewRequest request)
        {
            var response = await _client.PostAsync($"reviews/doctors/{request.DoctorId}", request);

            response.EnsureSuccessStatusCode();
        }
    }
}
