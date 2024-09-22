using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.History
{
    public class MockHistoryStore : IHistoryStore
    {
        private static Random _random = new Random();

        public Task<List<CheckPointHistoryDto>> GetHistoriesAsync()
        {
            throw new NotImplementedException();

        }

        private static CarDto GetMockCar()
        {
            var car = new CarDto
            {
                Model = "Spark",
                Color = "Yashil",
                Number = "AD523C",
                ManufacturedYear = 2016,
                Mileage = 500,
                YearlyDistanceLimit = 2500,
                MonthlyDistanceLimit = 750,
                CurrentMonthMileage = 500,
                AverageFuelConsumption = 50,
                FuelCapacity = 70,
                RemainingFuel = 20,
                Status = CarStatus.Free
            };

            return car;
        }

        private static RideDetailDto GetMockRideDetail()
        {
            var rideDetail = new RideDetailDto();

            rideDetail.DistanceTravelled = _random.Next(10, 300);
            rideDetail.DistanceTravelledAdjustment = _random.Next(0, 500);

            rideDetail.FuelConsumption = _random.Next(10, 100);
            rideDetail.FuelConsumptionAdjustment = _random.Next(0, 150);

            return rideDetail;
        }
    }
}
