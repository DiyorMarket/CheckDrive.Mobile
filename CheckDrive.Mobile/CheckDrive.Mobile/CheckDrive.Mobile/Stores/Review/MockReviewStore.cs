using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Review
{
    public class MockReviewStore : IReviewStore
    {
        public async Task<CheckPointDto> GetCheckPointAsync(bool forceRefresh = false)
        {
            var loadTime = forceRefresh ? TimeSpan.FromSeconds(1.5) : TimeSpan.FromSeconds(0.5);
            await Task.Delay(loadTime);

            var checkPoint = FakeDataGenerator.GetCheckPoint();

            return checkPoint;
        }

        public Task<List<ReviewDto>> GetReviews()
        {
            throw new NotImplementedException();
        }
    }
}
