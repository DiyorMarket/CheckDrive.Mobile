using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Doctor
{
    public sealed class MockDoctorStore : IDoctorStore
    {
        public async Task CreateAsync(DoctorReview review)
        {
            if (new Random().Next(100) % 2 == 0)
            {
                throw new Exception();
            }
            await Task.Delay(1250);
        }

        public Task CreateAsync(DoctorReviewRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DriverDto>> GetDriversAsync()
        {
            var fakeDriver = FakeDataGenerator.GetDrivers();
            var driversCount = new Random().Next(5, 20);

            var drivers = fakeDriver.Generate(driversCount).ToList();

            await Task.Delay(1000);

            return drivers;
        }

        public async Task<List<DoctorHistory>> GetReviewHistoryAsync(bool forceRefresh = false)
        {
            var fakeHistory = FakeDataGenerator.GetDoctorHistory();
            var historyCount = new Random().Next(5, 20);

            var histories = fakeHistory.Generate(historyCount).ToList();

            await Task.Delay(1250);

            return histories;
        }

        public Task<List<DoctorReview>> GetReviews(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }
    }
}
