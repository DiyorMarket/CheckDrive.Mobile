using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Stores.Car
{
    public class CarStore : ICarStore
    {
        private readonly ApiClient _client;

        public CarStore()
        {
            _client = DependencyService.Get<ApiClient>();
        }

        public async Task<List<CarDto>> GetAvailableCarsAsync()
        {
            var cars = await _client.GetAsync<List<CarDto>>($"cars?Status={CarStatus.Free}");
            return cars;
        }
    }
}
