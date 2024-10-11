using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Review;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Mechanic
{
    public class MockMechanicHandoverStore : IMechanicHandoverStore
    {
        public async Task CreateReviewAsync(MechanicHandoverReview review)
        {
            if (new Random().Next(0, 100) % 2 == 0)
            {
                throw new Exception("Random exception");
            }

            await Task.Delay(1500);
        }

        public Task<List<DriverDto>> GetDriversForReviewAsync()
        {
            throw new NotImplementedException();
        }
    }
}
