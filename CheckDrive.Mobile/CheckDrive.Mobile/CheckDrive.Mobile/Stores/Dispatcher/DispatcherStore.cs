using CheckDrive.Mobile.Models.Dispatcher;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Stores.Account;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Stores.Dispatcher
{
    public class DispatcherStore : IDispatcherStore
    {
        private readonly IAccountStore _accountStore;
        private readonly ApiClient _client;

        public DispatcherStore()
        {
            _client = DependencyService.Get<ApiClient>();
            _accountStore = DependencyService.Get<IAccountStore>();
        }

        public async Task CreateReveiwAsync(DispatcherReviewRequest request)
        {
            var response = await _client.PostAsync($"reviews/dispatchers/{request.DispatcherId}", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<DispatcherHistory>> GetHistoryAsync()
        {
            var dispatcherId = await _accountStore.GetUserIdAsync();
            var histories = await _client.GetAsync<List<DispatcherHistory>>($"reviews/histories/dispatchers/{dispatcherId}");

            return histories;
        }
    }
}
