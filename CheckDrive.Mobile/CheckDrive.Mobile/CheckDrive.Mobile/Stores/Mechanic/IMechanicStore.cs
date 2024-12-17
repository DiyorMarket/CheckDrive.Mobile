using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Mechanic;
using CheckDrive.Mobile.Models.Mechanic.Acceptance;
using CheckDrive.Mobile.Models.Mechanic.Handover;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Mechanic
{
    public interface IMechanicStore
    {
        Task<List<DriverDto>> GetDriversForHandoverReviewAsync();
        Task<List<DriverDto>> GetDriversForAcceptanceReviewAsync();
        Task<List<MechanicHistoryDto>> GetHistoriesAsync();
        Task CreateReviewAsync(MechanicHandoverRequest request);
        Task CreateReviewAsync(MechanicAcceptanceRequest request);
    }
}
