using CheckDrive.Mobile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Car
{
    public interface ICarStore
    {
        Task<List<CarDto>> GetAvailableCarsAsync();
    }
}
