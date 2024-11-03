using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Stores.Dispatcher
{
    public class DispatcherStore : IDispatcherStore
    {
        private readonly ApiClient _client;

        public DispatcherStore()
        {
            _client = DependencyService.Get<ApiClient>();
        }

        public async Task CreateReveiwAsync(DispatcherReview review)
        {
            var response = await _client.PostAsync($"reviews/dispatchers/{review.ReviewerId}", review);
            response.EnsureSuccessStatusCode();
        }
    }
}
