using System.Collections.Generic;
using System.Threading.Tasks;
using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Driver;

namespace CheckDrive.Mobile.Stores.Doctor
{
    public interface IDoctorStore
    {
        Task<List<DriverDto>> GetDriversAsync();
        Task<List<DoctorReview>> GetReviews(bool forceRefresh = false);
        Task CreateAsync(DoctorReviewRequest request);
    }
}
