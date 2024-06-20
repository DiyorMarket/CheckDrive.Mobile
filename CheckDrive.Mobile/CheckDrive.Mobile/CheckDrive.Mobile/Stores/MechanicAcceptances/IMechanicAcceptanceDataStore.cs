using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Mobile.Responses;
using System;
using System.Threading.Tasks;

namespace CheckDrive.Web.Stores.MechanicAcceptances
{
    public interface IMechanicAcceptanceDataStore
    {
        Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(int driverId, string sortString);
        Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(DateTime date);
        Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync();
        Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesByDriverIdAsync(int driverId);
        Task<MechanicAcceptanceDto> GetMechanicAcceptanceAsync(int id);
        Task<MechanicAcceptanceDto> CreateMechanicAcceptanceAsync(MechanicAcceptanceForCreateDto mechanicAcceptance);
    }
}
