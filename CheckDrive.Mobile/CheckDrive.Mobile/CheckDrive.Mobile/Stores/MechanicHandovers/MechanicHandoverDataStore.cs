using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Mobile.Responses;
using CheckDrive.Mobile.Services;
using CheckDrive.Web.Stores.MechanicHandovers;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.MechanicHandovers
{
    public class MechanicHandoverDataStore : IMechanicHandoverDataStore
    {
        private readonly ApiClient _api;

        public MechanicHandoverDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync()
        {
            var response = await _api.GetAsync("mechanics/handovers");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch mechanic handovers.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetMechanicHandoverResponse>(json);

            return result;
        }

        public async Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(DateTime date)
        {
            StringBuilder query = new StringBuilder("");

            if (date != DateTime.MinValue)
            {
                query.Append($"Date={date.Date}&");
            }

            var response = await _api.GetAsync("mechanics/handovers?" + query.ToString());
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch mechanic handovers.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetMechanicHandoverResponse>(json);

            return result;
        }

        public async Task<GetMechanicHandoverResponse> GetMechanicHandoversByDriverIdAsync(int driverId)
        {
            var response = await _api.GetAsync("mechanics/handovers?DriverId=" + driverId + "&OrderBy=datedesc");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch mechanic handovers.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetMechanicHandoverResponse>(json);

            return result;
        }

        public async Task<MechanicHandoverDto> GetMechanicHandoverAsync(int id)
        {
            var response = await _api.GetAsync($"mechanics/handover/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch mechanic handover with id: {id}.");
            }

            var json = await
            response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MechanicHandoverDto>(json);

            return result;
        }

        public async Task<MechanicHandoverDto> CreateMechanicHandoverAsync(MechanicHandoverForCreateDto mechanicHandover)
        {
            var json = JsonConvert.SerializeObject(mechanicHandover);
            var response = await _api.PostAsync("mechanics/handover", json);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating mechanic handover.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MechanicHandoverDto>(jsonResponse);
        }
    }
}
