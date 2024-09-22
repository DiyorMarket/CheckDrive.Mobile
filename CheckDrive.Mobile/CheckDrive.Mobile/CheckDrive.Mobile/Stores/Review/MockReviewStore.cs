using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Review
{
    public class MockReviewStore : IReviewStore
    {
        public async Task<CheckPointDto> GetCheckPointAsync(bool forceRefresh = false)
        {
            var loadTime = forceRefresh ? TimeSpan.FromSeconds(1.5) : TimeSpan.FromSeconds(0.5);
            await Task.Delay(loadTime);

            var reviews = GetMockReviews();
            var car = GetMockCar();
            var checkPoint = new CheckPointDto
            {
                Status = CheckPointStatus.InProgress,
                Stage = CheckPointStage.DoctorReview,
                Car = car,
                Reviews = reviews,
            };

            return checkPoint;
        }

        public Task<List<ReviewDto>> GetReviews()
        {
            throw new NotImplementedException();
        }

        private static List<ReviewDto> GetMockReviews()
        {
            var reviews = new List<ReviewDto>();
            reviews.Add(new ReviewDto
            {
                Id = 1,
                Date = DateTime.Now,
                Notes = "Doctor Review.",
                Status = ReviewStatus.Approved,
                Type = ReviewType.DoctorReview
            });
            reviews.Add(new ReviewDto
            {
                Id = 1,
                Date = DateTime.Now,
                Notes = "Mechanic Handover.",
                Status = ReviewStatus.Approved,
                Type = ReviewType.MechanicHandover
            });
            reviews.Add(new ReviewDto
            {
                Id = 1,
                Date = DateTime.Now,
                Notes = "Operator Review.",
                Status = ReviewStatus.InProgress,
                Type = ReviewType.OperatorReview
            });
            reviews.Add(new ReviewDto
            {
                Id = 1,
                Date = DateTime.Now,
                Notes = "Mechanic Review.",
                Status = ReviewStatus.NotStarted,
                Type = ReviewType.MechanicAcceptance
            });
            reviews.Add(new ReviewDto
            {
                Id = 1,
                Date = DateTime.Now,
                Notes = "Dispatcher Review.",
                Status = ReviewStatus.NotStarted,
                Type = ReviewType.DispatcherReview
            });

            return reviews;
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
    }
}
