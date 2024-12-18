using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Stores.CheckPoint
{
    public class CheckPointStore : ICheckPointStore
    {
        private readonly ApiClient _client;

        public CheckPointStore()
        {
            _client = DependencyService.Get<ApiClient>();
        }

        public async Task<List<CheckPointDto>> GetCheckPointsAsync(CheckPointStage stage)
        {
            var checkPoints = await _client.GetAsync<List<CheckPointDto>>($"checkPoints?stage={stage}");

            return checkPoints;
        }
    }
}
