using CheckDrive.ApiContracts.Car;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckDrive.Web.Stores.Cars
{
    public class MockCarDataStore
    {
        private readonly List<CarDto> _Cars;

        public MockCarDataStore()
        {
            _Cars = new List<CarDto>
            {
                new CarDto { Id = 1, Model = "Toyota Camry", Color = "Red", Number = "ABC123", MeduimFuelConsumption = 8.5, FuelTankCapacity = 60, ManufacturedYear = 2020 },
                new CarDto { Id = 2, Model = "Honda Civic", Color = "Blue", Number = "DEF456", MeduimFuelConsumption = 7.2, FuelTankCapacity = 55, ManufacturedYear = 2019 },
            };
        }

        public async Task<List<CarDto>> GetCarsAsync()
        {
            await Task.Delay(100); 
            return _Cars.ToList();
        }

        public async Task<CarDto> GetCarAsync(int id)
        {
            await Task.Delay(100);
            return _Cars.FirstOrDefault(c => c.Id == id);
        }

        public async Task<CarDto> CreateCarAsync(CarForCreateDto Car)
        {
            await Task.Delay(100); 
            
            return new CarDto()
            {
                Id = _Cars.Count + 1,
                Model = Car.Model,
                Color = Car.Color,
                ManufacturedYear = Car.ManufacturedYear,
                FuelTankCapacity = Car.FuelTankCapacity,
                MeduimFuelConsumption = Car.MeduimFuelConsumption,
                Number = Car.Number,
            };
        }

        public async Task<CarDto> UpdateCarAsync(int id, CarForUpdateDto Car)
        {
            await Task.Delay(100);
            var existingCar = _Cars.FirstOrDefault(c => c.Id == id);
            if (existingCar != null)
            {
                existingCar.Model = Car.Model;
                existingCar.Color = Car.Color;
                existingCar.Number = Car.Number;
                existingCar.MeduimFuelConsumption = Car.MeduimFuelConsumption;
                existingCar.FuelTankCapacity = Car.FuelTankCapacity;
                existingCar.ManufacturedYear = Car.ManufacturedYear;
            }
            return existingCar;
        }

        public async Task DeleteCarAsync(int id)
        {
            await Task.Delay(100);
            var CarToRemove = _Cars.FirstOrDefault(c => c.Id == id);
            if (CarToRemove != null)
            {
                _Cars.Remove(CarToRemove);
            }
        }
    }
}
