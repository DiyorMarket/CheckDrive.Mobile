using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Driver
{
    public interface IDriverStore
    {
        Task<CheckPointDto> GetCurrentCheckPointAsync();
        Task<List<DriverDto>> GetDriversForReviewAsync(CheckPointStage stage);
        Task SendReviewConfirmationAsync(DriverReviewResponse confirmation);
        Task<List<CheckPointHistoryDto>> GetHistoriesAsync();
    }
}
