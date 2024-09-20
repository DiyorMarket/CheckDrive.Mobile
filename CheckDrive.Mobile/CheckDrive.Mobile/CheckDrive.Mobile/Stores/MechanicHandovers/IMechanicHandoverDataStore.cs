using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Mobile.Responses;
using System;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.MechanicHandovers
{
    public interface IMechanicHandoverDataStore
    {
        Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync();
        Task<GetMechanicHandoverResponse> GetMechanicHandoversByDriverIdAsync(int driverId);
        Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(DateTime date);
        Task<MechanicHandoverDto> GetMechanicHandoverAsync(int id);
    }
}
