using CheckDrive.ApiContracts.Car;
using CheckDrive.Mobile.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Web.Stores.Cars
{
    public interface ICarDataStore
    {
        Task<GetCarResponse> GetCarsAsync();
        Task<CarDto> GetCarAsync(int id);
        Task<CarDto> CreateCarAsync(CarForCreateDto Car);
    }
}
