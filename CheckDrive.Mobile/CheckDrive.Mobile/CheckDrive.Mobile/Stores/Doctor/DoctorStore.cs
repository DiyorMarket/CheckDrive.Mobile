using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Stores.Doctor
{
    public class DoctorStore : IDoctorStore
    {
        private readonly ApiClient _client;

        public DoctorStore()
        {
            _client = DependencyService.Get<ApiClient>();
        }

        public async Task CreateAsync(DoctorReview review)
        {
            var response = await _client.PostAsync("reviews/doctors/13", review);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<DriverDto>> GetDriversAsync()
        {
            var drivers = await _client.GetAsync<List<DriverDto>>("drivers");

            return drivers;
        }

        public Task<List<DoctorHistory>> GetReviewHistoryAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }
    }
}
