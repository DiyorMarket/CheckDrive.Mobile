using CheckDrive.ApiContracts.Driver;
using CheckDrive.Mobile.Responses;
using CheckDrive.Mobile.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Drivers
{
    public class DriverDataStore : IDriverDataStore
    {
        private readonly ApiClient _api;

        public DriverDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetDriverResponse> GetDriversAsync(int accountId)
        {
            StringBuilder query = new StringBuilder("");

            if (!accountId.Equals(0))
            {
                query.Append($"accountId={accountId}");
            }

            var response = await _api.GetAsync("drivers?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetDriverResponse>(json);

            return result;
        }

        public async Task<List<DriverHistoryDto>> GetDriverHistoryDtosAsync(int driverId)
        {
            StringBuilder query = new StringBuilder("");

            if (!driverId.Equals(0))
            {
                query.Append($"driverId={driverId}");
            }

            var response = await _api.GetAsync("drivers/driverHistories?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<DriverHistoryDto>>(json);

            return result;
        }

        public async Task<DriverDto> GetDriverAsync(int id)
        {
            var response = await _api.GetAsync($"drivers?AccountId={id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DriverDto>(json);

            return result;
        }
    }
}
