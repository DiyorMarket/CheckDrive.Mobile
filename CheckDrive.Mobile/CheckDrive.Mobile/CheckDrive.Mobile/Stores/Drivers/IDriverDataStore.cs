using CheckDrive.ApiContracts.Driver;
using CheckDrive.Mobile.Responses;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Drivers
{
    public interface IDriverDataStore
    {
        Task<GetDriverResponse> GetDriversAsync(int accountId);
        Task<DriverDto> GetDriverAsync(int id);
        Task<List<DriverHistoryDto>> GetDriverHistoryDtosAsync(int driverId);
    }
}