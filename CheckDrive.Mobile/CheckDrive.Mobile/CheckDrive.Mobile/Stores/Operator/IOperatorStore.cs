using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Review;
using CheckDrive.Mobile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Operator
{
    public interface IOperatorStore
    {
        Task<List<DriverDto>> GetDriversAsync();
        Task<List<DoctorHistory>> GetReviewHistoryAsync(bool forceRefresh = false);
        Task CreateAsync(OperatorReview operatorReview);
    }
}
