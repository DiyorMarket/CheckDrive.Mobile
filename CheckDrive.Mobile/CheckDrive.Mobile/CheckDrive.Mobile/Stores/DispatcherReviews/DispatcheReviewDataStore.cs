using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Mobile.Responses;
using CheckDrive.Mobile.Services;
using CheckDrive.Web.Stores.DispatcherReviews;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.DispatcherReviews
{
    public class DispatcheReviewDataStore : IDispatcherReviewDataStore
    {
        private readonly ApiClient _api;

        public DispatcheReviewDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetDispatcherReviewResponse> GetDispatcherReviewsAsync()
        {
            StringBuilder query = new StringBuilder("");

            var response = await _api.GetAsync("dispatchers/reviews?" + query.ToString());
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch dispatcher reviews.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetDispatcherReviewResponse>(json);

            return result;
        }

        public async Task<DispatcherReviewDto> GetDispatcherReviewAsync(int id)
        {
            var response = await _api.GetAsync($"dispatchers/reviews/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch dispatcher review with id: {id}.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DispatcherReviewDto>(json);

            return result;
        }

        public async Task<DispatcherReviewDto> CreateDispatcherReviewAsync(DispatcherReviewForCreateDto dispatcherReview)
        {
            var json = JsonConvert.SerializeObject(dispatcherReview);
            var response = await _api.PostAsync("dispatchers/reviews", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating dispatcher review.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DispatcherReviewDto>(jsonResponse);
        }
    }
}
