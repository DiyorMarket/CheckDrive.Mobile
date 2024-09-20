using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Mobile.Responses;
using CheckDrive.Mobile.Services;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.MechanicAcceptances
{
    public class MechanicAcceptanceDataStore : IMechanicAcceptanceDataStore
    {
        private readonly ApiClient _api;

        public MechanicAcceptanceDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(int driverId, string sortString)
        {
            StringBuilder query = new StringBuilder("");

            if (!driverId.Equals(0))
            {
                query = query.Append($"DriverId={driverId}&");
            }

            if (!string.IsNullOrEmpty(sortString))
            {
                query = query.Append($"Sort={sortString}&");
            }

            var response = await _api.GetAsync("mechanics/acceptances?" + query.ToString());
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch mechanic acceptance.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetMechanicAcceptanceResponse>(json);

            return result;
        }

        public async Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesByDriverIdAsync(int driverId)
        {
            var response = await _api.GetAsync("mechanics/acceptances?DriverId=" + driverId + "&OrderBy=datedesc");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch mechanic acceptance.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetMechanicAcceptanceResponse>(json);

            return result;
        }

        public async Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(DateTime date)
        {
            StringBuilder query = new StringBuilder("");

            if (date > DateTime.MinValue)
            {
                query = query.Append($"Date={date.Month}/{date.Day}/{date.Year}");
            }

            var response = await _api.GetAsync("mechanics/acceptances?roleId=10&" + query.ToString());
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch mechanic acceptance.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetMechanicAcceptanceResponse>(json);

            return result;
        }

        public async Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync()
        {
            var response = await _api.GetAsync("mechanics/acceptances");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch mechanic acceptance.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetMechanicAcceptanceResponse>(json);

            return result;
        }

        public async Task<MechanicAcceptanceDto> GetMechanicAcceptanceAsync(int id)
        {
            var response = await _api.GetAsync($"mechanics/acceptances/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch mechanic acceptance with id: {id}.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MechanicAcceptanceDto>(json);

            return result;
        }
    }
}
