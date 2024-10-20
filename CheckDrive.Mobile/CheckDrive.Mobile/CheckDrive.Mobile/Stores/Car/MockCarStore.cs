using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Car
{
    public class MockCarStore : ICarStore
    {
        public async Task<List<CarDto>> GetAvailableCarsAsync()
        {
            var fakeCarGenerator = FakeDataGenerator.GetCar();
            var carsCount = new Random().Next(5, 20);

            var cars = fakeCarGenerator.Generate(carsCount).ToList();

            await Task.Delay(1500);

            return cars;
        }
    }
}
