using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Mechanic;
using CheckDrive.Mobile.Models.Mechanic.Acceptance;
using CheckDrive.Mobile.Models.Mechanic.Handover;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Mechanic
{
    public class MockMechanicStore : IMechanicStore
    {
        public async Task CreateReviewAsync(MechanicHandoverReview review)
        {
            if (new Random().Next(0, 100) % 2 == 0)
            {
                throw new Exception("Random exception");
            }

            await Task.Delay(1500);
        }

        public async Task CreateReviewAsync(MechanicAcceptanceReview review)
        {
            if (new Random().Next(0, 100) % 2 == 0)
            {
                throw new Exception("Random exception");
            }

            await Task.Delay(1500);
        }

        public async Task<List<MechanicHistoryDto>> GetHistoriesAsync()
        {
            var count = new Random().Next(5, 20);
            var generator = FakeDataGenerator.GetMechanicHistory();
            var histories = generator.Generate(count);

            await Task.Delay(1000);

            return histories;
        }

        public Task<List<DriverDto>> GetDriversForHandoverReviewAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<DriverDto>> GetDriversForAcceptanceReviewAsync()
        {
            throw new NotImplementedException();
        }

        public Task CreateReviewAsync(MechanicHandoverRequest request)
        {
            throw new NotImplementedException();
        }

        public Task CreateReviewAsync(MechanicAcceptanceRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
