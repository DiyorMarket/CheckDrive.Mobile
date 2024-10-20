using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Review;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Doctor
{
    public interface IDoctorStore
    {
        Task<List<DriverDto>> GetDriversAsync();
        Task<List<DoctorHistory>> GetReviewHistoryAsync(bool forceRefresh = false);
        Task CreateAsync(DoctorReview review);
    }
}
