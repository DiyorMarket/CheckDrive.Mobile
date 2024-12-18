using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Operator;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Stores.Account;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Stores.Operator
{
    public class OperatorStore : IOperatorStore
    {
        private readonly IAccountStore _accountStore;
        private readonly ApiClient _client;

        public OperatorStore()
        {
            _accountStore = DependencyService.Get<IAccountStore>();
            _client = DependencyService.Get<ApiClient>();
        }

        public async Task<List<OilMark>> GetOilMarksAsync()
        {
            var oilMarks = await _client.GetAsync<List<OilMark>>("oilMarks");

            return oilMarks;
        }

        public async Task CreateReviewAsync(OperatorReviewRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = await _client.PostAsync($"reviews/operators/{request.OperatorId}", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<OperatorHistoryDto>> GetHistoriesAsync()
        {
            var operatorId = await _accountStore.GetUserIdAsync();
            var histories = await _client.GetAsync<List<OperatorHistoryDto>>($"reviews/histories/operators/{operatorId}");

            return histories;
        }
    }
}
