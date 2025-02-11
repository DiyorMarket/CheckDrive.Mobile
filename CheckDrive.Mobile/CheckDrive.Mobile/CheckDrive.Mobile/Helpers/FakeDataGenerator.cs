﻿using Bogus;
using Bogus.Extensions.Poland;
using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Account;
using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Mechanic;
using System;
using System.Collections.Generic;

namespace CheckDrive.Mobile.Helpers
{
    public static class FakeDataGenerator
    {
        private static readonly Faker faker = new Faker();

        public static Faker<CarDto> GetCar() => new Faker<CarDto>()
            .RuleFor(x => x.Model, f => f.Vehicle.Model())
            .RuleFor(x => x.Color, f => f.Commerce.Color())
            .RuleFor(x => x.Number, f => f.Vehicle.Vin())
            .RuleFor(x => x.ManufacturedYear, f => f.Random.Number(2000, 2020))
            .RuleFor(x => x.Mileage, f => f.Random.Number(0, 1000))
            .RuleFor(x => x.YearlyDistanceLimit, f => f.Random.Number(1000, 5000))
            .RuleFor(x => x.FuelCapacity, f => f.Random.Number(50, 150))
            .RuleFor(x => x.AverageFuelConsumption, (f, x) => f.Random.Decimal(50, x.FuelCapacity))
            .RuleFor(x => x.RemainingFuel, (f, x) => f.Random.Decimal(50, x.FuelCapacity))
            .RuleFor(x => x.MonthlyDistanceLimit, f => f.Random.Number(500, 1500))
            .RuleFor(x => x.CurrentMonthMileage, f => f.Random.Number(500, 1500))
            .RuleFor(x => x.Status, f => f.Random.Enum<CarStatus>());

        public static RideDetailDto GetRideDetail() => new Faker<RideDetailDto>()
            .RuleFor(x => x.DistanceTravelled, f => f.Random.Number(10, 300))
            .RuleFor(x => x.DistanceTravelledAdjustment, f => f.Random.Number(0, 500))
            .RuleFor(x => x.FuelConsumption, f => f.Random.Number(10, 150))
            .RuleFor(x => x.FuelConsumptionAdjustment, f => f.Random.Number(0, 300))
            .Generate();

        public static DoctorReviewDto GetDoctorReview() => new Faker<DoctorReviewDto>()
            .RuleFor(x => x.ReviewerId, f => f.Random.Number())
            .RuleFor(x => x.ReviewerName, f => f.Person.FullName)
            .RuleFor(x => x.DriverId, f => f.Random.Number())
            .RuleFor(x => x.DriverName, f => f.Person.FullName)
            .RuleFor(x => x.IsHealthy, f => f.Random.Bool())
            .Generate();

        public static CheckPointDto GetCheckPoint()
        {
            var car = GetCar().Generate();
            var reviews = new List<ReviewDto>();
            ReviewDto previousReview = null;

            foreach (ReviewType type in (ReviewType[])Enum.GetValues(typeof(ReviewType)))
            {
                if (previousReview == null)
                {
                    var review = GetReview(type);
                    reviews.Add(review);
                    previousReview = review;

                    continue;
                }

                if (previousReview.Status != ReviewStatus.Approved)
                {
                    var review = GetReview(type, ReviewStatus.NotStarted);
                    reviews.Add(review);
                    previousReview = review;
                }
                else
                {
                    var review = GetReview(type);
                    reviews.Add(review);
                    previousReview = review;
                }
            }

            // Adjust time
            for (int i = 1; i < reviews.Count; i++)
            {
                reviews[i].Date = reviews[i - 1].Date.AddMinutes(faker.Random.Number(10, 20));
            }

            var checkPoint = new CheckPointDto();
            checkPoint.StartDate = reviews[0].Date;
            checkPoint.Reviews = reviews;
            // checkPoint.DoctorReview = GetDoctorReview();

            if (checkPoint.Reviews.TrueForAll(x => x.Status == ReviewStatus.Approved))
            {
                checkPoint.Status = CheckPointStatus.Completed;
                checkPoint.Stage = CheckPointStage.DispatcherReview;
            }

            foreach (var review in checkPoint.Reviews)
            {
                if (review.Status == ReviewStatus.InProgress)
                {
                    checkPoint.Status = CheckPointStatus.InProgress;
                    checkPoint.Stage = GetStage(review.Type);
                }
                else if (review.Status == ReviewStatus.Rejected)
                {
                    checkPoint.Status = CheckPointStatus.Interrupted;
                    checkPoint.Stage = GetStage(review.Type);
                }
            }

            return checkPoint;
        }

        public static CheckPointHistoryDto GetCheckPointHistory()
        {
            var checkPoint = GetCheckPoint();
            var rideDetail = GetRideDetail();
            var history = new CheckPointHistoryDto();
            history.Id = faker.Random.Number(1, 10000);
            history.Stage = checkPoint.Stage;
            history.Status = checkPoint.Status;
            history.StartDate = checkPoint.StartDate;
            history.RideDetail = rideDetail;
            history.CarDetail = checkPoint.Car;
            history.Reviews = checkPoint.Reviews;

            return history;
        }

        public static AccountDto GetAccount() => new Faker<AccountDto>()
            .RuleFor(x => x.FirstName, f => f.Person.FirstName)
            .RuleFor(x => x.LastName, f => f.Person.LastName)
            .RuleFor(x => x.UserName, (f, x) => f.Internet.UserName(x.FirstName, x.LastName))
            .RuleFor(x => x.Passport, f => f.Person.Pesel())
            .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber("+998 9# ###-##-##"))
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.Address, f => f.Address.FullAddress())
            .RuleFor(x => x.Birthdate, f => f.Person.DateOfBirth);

        public static Faker<DriverDto> GetDrivers() => new Faker<DriverDto>()
            .RuleFor(x => x.Id, f => f.Random.Number())
            .RuleFor(x => x.FullName, f => f.Person.FullName);

        public static Faker<DoctorHistory> GetDoctorHistory() => new Faker<DoctorHistory>()
            .RuleFor(x => x.Id, f => f.Random.Number())
            .RuleFor(x => x.DriverId, f => f.Random.Number())
            .RuleFor(x => x.DriverName, f => f.Person.FullName)
            .RuleFor(x => x.Date, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(-4)))
            .RuleFor(x => x.IsApproved, f => f.Random.Bool());

        public static Faker<MechanicHistoryDto> GetMechanicHistory() => new Faker<MechanicHistoryDto>()
            .RuleFor(x => x.CheckPointId, f => f.Random.Number(1, 1_000_000))
            .RuleFor(x => x.DriverId, f => f.Random.Number(1, 1_000_000))
            .RuleFor(x => x.DriverName, f => f.Person.FullName)
            .RuleFor(x => x.CarId, f => f.Random.Number(1, 1_000_000))
            .RuleFor(x => x.CarDetails, f => f.Vehicle.Model() + " " + f.Vehicle.Vin())
            .RuleFor(x => x.Notes, f => f.Lorem.Sentence())
            .RuleFor(x => x.Status, f => f.Random.Enum<ReviewStatus>())
            .RuleFor(x => x.Date, f => f.Date.Between(DateTime.Now.AddDays(-10), DateTime.Now));

        private static CheckPointStage GetStage(ReviewType type)
        {
            switch (type)
            {
                case ReviewType.DoctorReview:
                    return CheckPointStage.DoctorReview;
                case ReviewType.MechanicHandover:
                    return CheckPointStage.MechanicHandover;
                case ReviewType.OperatorReview:
                    return CheckPointStage.OperatorReview;
                case ReviewType.MechanicAcceptance:
                    return CheckPointStage.MechanicAcceptance;
                case ReviewType.DispatcherReview:
                    return CheckPointStage.DispatcherReview;
                default:
                    return CheckPointStage.DoctorReview;
            }
        }

        private static ReviewDto GetReview(ReviewType type, ReviewStatus? status = null) => new Faker<ReviewDto>()
            .RuleFor(x => x.Id, f => f.Random.Number(1, 10000))
            .RuleFor(x => x.Notes, f => f.Lorem.Sentence(4))
            .RuleFor(x => x.ReviewerName, f => f.Person.FullName)
            .RuleFor(x => x.Date, f => f.Date.Between(DateTime.Now.AddDays(-20), DateTime.Now))
            .RuleFor(x => x.Status, f => status == null ? GetStatus() : status)
            .RuleFor(x => x.Type, type)
            .Generate();

        private static ReviewStatus GetStatus()
        {
            var statusRandom = faker.Random.Int(0, 100);

            if (statusRandom < 85)
            {
                return ReviewStatus.Approved;
            }

            if (statusRandom < 90)
            {
                return ReviewStatus.Rejected;
            }

            if (statusRandom < 95)
            {
                return ReviewStatus.Rejected;
            }

            return ReviewStatus.InProgress;
        }
    }
}
