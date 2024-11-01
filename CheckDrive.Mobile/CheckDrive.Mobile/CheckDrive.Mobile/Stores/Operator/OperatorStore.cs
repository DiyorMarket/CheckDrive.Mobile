using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Stores.Operator
{
    public class OperatorStore : IOperatorStore
    {
        private readonly ApiClient _client;

        public OperatorStore()
        {
            _client = DependencyService.Get<ApiClient>();
        }

        public async Task<List<OilMark>> GetOilMarksAsync()
        {
            var oilMarks = await _client.GetAsync<List<OilMark>>("oilMarks");

            return oilMarks;
        }

        public async Task CreateReviewAsync(OperatorReview review)
        {
            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }

            var response = await _client.PostAsync($"reviews/operators/{review.ReviewerId}", review);
            response.EnsureSuccessStatusCode();
        }
    }
}
