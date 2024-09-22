using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;

namespace CheckDrive.Mobile.Models
{
    public class CheckPointHistoryDto
    {
        public int Id { get; set; }
        public CheckPointStage Stage { get; set; }
        public CheckPointStatus Status { get; set; }
        public DateTime Date { get; set; }
        public RideDetailDto RideDetail { get; set; }
        public CarDto CarDetail { get; set; }
        public List<ReviewDto> Reviews { get; set; }

        public string IconSource => GetIconSource(Status);
        public string CarDetails => GetCarDetails(CarDetail);
        public string RideDetails => GetRideDetails(RideDetail);

        private static string GetIconSource(CheckPointStatus status)
        {
            switch (status)
            {
                case CheckPointStatus.Completed:
                    return "icon_success.png";
                case CheckPointStatus.InProgress:
                    return "icon_warning.png";
                case CheckPointStatus.PendingManagerReview:
                    return "icon_warning.png";
                case CheckPointStatus.AutomaticallyClosed:
                    return "icon_warning.png";
                case CheckPointStatus.InterruptedWithRejection:
                    return "icon_error.png";
                default:
                    return "icon_error.png";
            }
        }

        private static string GetCarDetails(CarDto car)
        {
            if (car == null)
            {
                return "";
            }

            return $"{car.Color} {car.Model}, {car.ManufacturedYear}";
        }

        private static string GetRideDetails(RideDetailDto rideDetail)
        {
            if (rideDetail == null)
            {
                return "";
            }

            return $"{GetDistanceDetail(rideDetail)}, {GetFuelConsumptionDetail(rideDetail)}";
        }

        private static string GetDistanceDetail(RideDetailDto rideDetail)
        {
            if (rideDetail.DistanceTravelledAdjustment.HasValue)
            {
                return $"{rideDetail.DistanceTravelled} km ({rideDetail.DistanceTravelledAdjustment} km)";
            }

            return $"{rideDetail.DistanceTravelled} km";
        }

        private static string GetFuelConsumptionDetail(RideDetailDto rideDetail)
        {
            if (rideDetail.FuelConsumptionAdjustment.HasValue)
            {
                return $"{rideDetail.FuelConsumption} litr ({rideDetail.FuelConsumptionAdjustment} litr)";
            }

            return $"{rideDetail.FuelConsumptionAdjustment} litr";
        }
    }
}
