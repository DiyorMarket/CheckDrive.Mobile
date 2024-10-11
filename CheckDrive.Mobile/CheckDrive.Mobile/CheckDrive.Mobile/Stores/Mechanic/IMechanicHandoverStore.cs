using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Review;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Mechanic
{
    public interface IMechanicHandoverStore
    {
        Task<List<DriverDto>> GetDriversForReviewAsync();
        Task CreateReviewAsync(MechanicHandoverReview review);
        Task CreateReviewAsync(MechanicAcceptanceReview review);
    }
}
