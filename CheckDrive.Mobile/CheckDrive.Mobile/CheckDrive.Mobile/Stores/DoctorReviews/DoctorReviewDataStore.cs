using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Mobile.Responses;
using CheckDrive.Mobile.Services;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.DoctorReviews
{
    public class DoctorReviewDataStore : IDoctorReviewDataStore
    {
        private readonly ApiClient _api;

        public DoctorReviewDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetDoctorReviewResponse> GetDoctorReviewsAsync()
        {
            var response = await _api.GetAsync("doctors/reviews");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch doctor reviews.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetDoctorReviewResponse>(json);

            return result;
        }

        public async Task<GetDoctorReviewResponse> GetDoctorReviewsAsync(DateTime date)
        {
            StringBuilder query = new StringBuilder("");

            if (date != null)
            {
                query = query.Append($"Date={date.Month}/{date.Day}/{date.Year}");
            }

            var response = await _api.GetAsync("doctors/reviews?roleId=10&" + query.ToString());
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch doctor reviews.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetDoctorReviewResponse>(json);

            return result;
        }

        public async Task<GetDoctorReviewResponse> GetDoctorReviewsByDriverIdAsync(int driverId)
        {
            var response = await _api.GetAsync("doctors/reviews?DriverId=" + driverId + "&OrderBy=datedesc");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch doctor reviews.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetDoctorReviewResponse>(json);

            return result;
        }

        public async Task<DoctorReviewDto> GetDoctorReviewAsync(int id)
        {
            var response = await _api.GetAsync("doctors/reviews?Id=" + id);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch doctor reviews.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DoctorReviewDto>(json);

            return result;
        }
    }
}
