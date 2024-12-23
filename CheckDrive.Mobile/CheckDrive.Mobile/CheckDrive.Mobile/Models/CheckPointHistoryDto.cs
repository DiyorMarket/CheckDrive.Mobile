using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Mechanic.Acceptance;
using CheckDrive.Mobile.Models.Mechanic.Handover;
using CheckDrive.Mobile.Models.Operator;
using System;
using System.Collections.Generic;

namespace CheckDrive.Mobile.Models
{
    public class CheckPointHistoryDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public CheckPointStage Stage { get; set; }
        public CheckPointStatus Status { get; set; }
        public DoctorReviewDto DoctorReview { get; set; }
        public MechanicHandoverReview MechanicHandover { get; set; }
        public OperatorReview OperatorReview { get; set; }
        public MechanicAcceptanceReview MechanicAcceptance { get; set; }

        // UI Properties
        public List<ReviewDto> Reviews { get; set; }
        public RideDetailDto RideDetail { get; set; }
        public CarDto CarDetail { get; set; }
        public string IconSource => GetIconSource(Status);
        public string CarDetails => GetCarDetails(CarDetail);
        public string RideDetails => GetRideDetails(RideDetail);
        public string HistoryDate => StartDate.ToString("dd/MM");
        public string Summary => GetSummary(Status);
        public string FormattedDistanceTravelled => $"{RideDetail.DistanceTravelled:N0} KM";
        public string FormattedDistanceTravelledAdjustment => RideDetail.DistanceTravelledAdjustment.HasValue
            ? $"({RideDetail.DistanceTravelledAdjustment.Value:N0} KM)"
            : string.Empty;
        public string FormattedFuelConsumption => $"{RideDetail.FuelConsumption:N0} L";
        public string FormattedFuelConsumptionAdjustment => RideDetail.FuelConsumptionAdjustment.HasValue
            ? $"({RideDetail.FuelConsumptionAdjustment.Value:N0} L)"
            : string.Empty;

        public CheckPointHistoryDto()
        {
            Reviews = new List<ReviewDto>();

            SetupReviews();
            SetupDetails();
        }

        private void SetupReviews()
        {
            if (DoctorReview != null)
            {
                var review = new ReviewDto()
                {
                    CheckPointId = Id,
                    Date = DoctorReview.Date,
                    Notes = DoctorReview.Notes,
                    ReviewerName = DoctorReview.ReviewerName,
                    Status = DoctorReview.Status,
                    Type = ReviewType.DoctorReview
                };
                Reviews.Add(review);
            }

            if (MechanicHandover != null)
            {
                var review = new ReviewDto()
                {
                    CheckPointId = Id,
                    Date = MechanicHandover.Date,
                    Notes = MechanicHandover.Notes,
                    ReviewerName = MechanicHandover.MechanicName,
                    // Status = MechanicHandover.Status,
                    Type = ReviewType.MechanicHandover
                };
                Reviews.Add(review);
            }

            if (OperatorReview != null)
            {
                var review = new ReviewDto()
                {
                    CheckPointId = Id,
                    Date = OperatorReview.Date,
                    Notes = OperatorReview.Notes,
                    ReviewerName = OperatorReview.OperatorName,
                    // Status = OperatorReview.Status,
                    Type = ReviewType.OperatorReview
                };
                Reviews.Add(review);
            }

            if (MechanicAcceptance != null)
            {
                var review = new ReviewDto()
                {
                    CheckPointId = Id,
                    Date = MechanicAcceptance.Date,
                    Notes = MechanicAcceptance.Notes,
                    ReviewerName = MechanicAcceptance.MechanicName,
                    // Status = MechanicAcceptance.Status,
                    Type = ReviewType.MechanicAcceptance
                };
                Reviews.Add(review);
            }
        }

        private void SetupDetails()
        {
            CarDetail = MechanicHandover.Car;
            RideDetail = new RideDetailDto
            {
                DistanceTravelled = MechanicAcceptance.FinalMileage - MechanicHandover.InitialMileage,
                FuelConsumption = OperatorReview.InitialOilAmount - MechanicAcceptance.FinalMileage,
            };
        }

        private static string GetIconSource(CheckPointStatus status)
        {
            switch (status)
            {
                case CheckPointStatus.Completed:
                    return "icon_success.png";
                case CheckPointStatus.InProgress:
                    return "icon_warning.png";
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
                    return "Sistema tomonidan yopilgan";
                default:
                    return "Noaniq xolatda";
            }
        }
    }
}
