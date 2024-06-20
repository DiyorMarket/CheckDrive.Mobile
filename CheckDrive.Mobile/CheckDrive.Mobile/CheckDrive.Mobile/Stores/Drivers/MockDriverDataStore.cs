using CheckDrive.ApiContracts.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckDrive.Web.Stores.Drivers
{
    public class MockDriverDataStore
    {
        private readonly List<DriverDto> _drivers;

        public MockDriverDataStore()
        {
            _drivers = new List<DriverDto>
            {
                new DriverDto {Id = 1, Login = "user1", Password = "password1", PhoneNumber = "123456789", FirstName = "John", LastName = "Doe", Birthdate = new DateTime(1990, 1, 1)  },
                new DriverDto { Id = 2, Login = "user2", Password = "password2", PhoneNumber = "987654321", FirstName = "Jane", LastName = "Siu", Birthdate = new DateTime(1995, 5, 15) },
            };
        }

        public async Task<List<DriverDto>> GetDrivers()
        {
            await Task.Delay(100);
            return _drivers.ToList();
        }

        public async Task<DriverDto> GetDriver(int id)
        {
            await Task.Delay(100);
            return _drivers.FirstOrDefault(d => d.Id == id);
        }

        public async Task<DriverDto> CreateDriver(DriverDto driver)
        {
            await Task.Delay(100);
            driver.Id = _drivers.Max(d => d.Id) + 1; 
            _drivers.Add(driver);
            return driver;
        }

        public async void DeleteDriver(int id)
        {
            await Task.Delay(100);
            var driverToRemove = _drivers.FirstOrDefault(d => d.Id == id);
            if (driverToRemove != null)
            {
                _drivers.Remove(driverToRemove);
            }
        }
    }
}
