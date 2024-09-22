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
        public string HistoryDate => Date.ToString("dd/MM");
        public string Summary => GetSummary(Status);
        public string FormattedDistanceTravelled => $"{RideDetail.DistanceTravelled:N0} KM";
        public string FormattedDistanceTravelledAdjustment => RideDetail.DistanceTravelledAdjustment.HasValue
            ? $"({RideDetail.DistanceTravelledAdjustment.Value:N0} KM)"
            : string.Empty;
        public string FormattedFuelConsumption => $"{RideDetail.FuelConsumption:N0} L";
        public string FormattedFuelConsumptionAdjustment => RideDetail.FuelConsumptionAdjustment.HasValue
            ? $"({RideDetail.FuelConsumptionAdjustment.Value:N0} L)"
            : string.Empty;

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

            return $"{car.Color} {car.Model} {car.Number}";
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

        private static string GetSummary(CheckPointStatus status)
        {
            switch (status)
            {
                case CheckPointStatus.InProgress:
                    return "Yakunlanmagan";
                case CheckPointStatus.Completed:
                    return "Yakunlangan";
                case CheckPointStatus.PendingManagerReview:
                    return "Menejer tekshiruvida";
                case CheckPointStatus.InterruptedWithRejection:
                    return "Muammo bilan to'xtatilgan";
                case CheckPointStatus.AutomaticallyClosed:
                    return "Sistema tomonidan yopilgan";
                default:
                    return "Noaniq xolatda";
            }
        }
    }
}
