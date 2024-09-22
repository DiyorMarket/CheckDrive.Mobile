using CheckDrive.Mobile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Review
{
    public interface IReviewStore
    {
        Task<List<ReviewDto>> GetReviews();
        Task<CheckPointDto> GetCheckPointAsync(bool forceRefresh = false);
    }
}
