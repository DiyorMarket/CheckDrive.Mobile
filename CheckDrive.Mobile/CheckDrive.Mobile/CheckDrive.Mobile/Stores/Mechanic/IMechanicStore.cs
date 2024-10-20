using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Mechanic;
using CheckDrive.Mobile.Models.Review;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Mechanic
{
    public interface IMechanicStore
    {
        Task<List<DriverDto>> GetDriversForReviewAsync();
        Task<List<MechanicHistoryDto>> GetHistoriesAsync();
        Task CreateReviewAsync(MechanicHandoverReview review);
        Task CreateReviewAsync(MechanicAcceptanceReview review);
    }
}
