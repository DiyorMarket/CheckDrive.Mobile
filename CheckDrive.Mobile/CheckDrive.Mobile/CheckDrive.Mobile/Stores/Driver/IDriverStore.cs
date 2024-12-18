using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Driver
{
    public interface IDriverStore
    {
        Task<List<DriverDto>> GetDriversAsync(DriverStatus status = DriverStatus.Available);
        Task<CheckPointDto> GetCurrentCheckPointAsync();
        Task SendReviewConfirmationAsync(ReviewConfirmationRequest request);
        Task<List<CheckPointHistoryDto>> GetHistoriesAsync();
    }
}
