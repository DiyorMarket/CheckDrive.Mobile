using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Driver
{
    public class MockDriverStore : IDriverStore
    {
        public Task<CheckPointDto> GetCurrentCheckPointAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<DriverDto>> GetDriversAsync(DriverStatus status = DriverStatus.Available)
        {
            await Task.Delay(500);

            var drivers = FakeDataGenerator.GetDrivers().Generate(10);

            return drivers;
        }

        public Task<List<CheckPointHistoryDto>> GetHistoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task SendReviewConfirmationAsync(ReviewConfirmationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
