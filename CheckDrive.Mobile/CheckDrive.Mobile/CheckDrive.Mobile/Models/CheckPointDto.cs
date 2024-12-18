using CheckDrive.Mobile.Models.Dispatcher;
using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Mechanic.Acceptance;
using CheckDrive.Mobile.Models.Mechanic.Handover;
using CheckDrive.Mobile.Models.Operator;
using System;
using System.Collections.Generic;

namespace CheckDrive.Mobile.Models
{
    public class CheckPointDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public CheckPointStage Stage { get; set; }
        public CheckPointStatus Status { get; set; }
        public DoctorReview DoctorReview { get; set; }
        public MechanicHandoverReview MechanicHandover { get; set; }
        public OperatorReview OperatorReview { get; set; }
        public MechanicAcceptanceReview MechanicAcceptance { get; set; }
        public DispatcherReview DispatcherReview { get; set; }

        public CarDto Car { get; set; }
        public DriverDto Driver { get; set; }

        public string DriverName => Driver?.FullName ?? "";
        public List<ReviewDto> Reviews { get; set; }

        public CheckPointDto()
        {
            Reviews = new List<ReviewDto>();

            SetupReviews();
        }

        public void SetupReviews()
        {
            if (DoctorReview != null)
            {
                var review = new ReviewDto()
                {
                    CheckPointId = Id,
                    Date = DoctorReview.Date,
                    Notes = DoctorReview.Notes,
                    ReviewerName = DoctorReview.DriverName,
                    Status = DoctorReview.IsHealthy ? ReviewStatus.Approved : ReviewStatus.Rejected,
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
                    Status = MechanicHandover.Status,
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
                    Status = OperatorReview.Status,
                    Type = ReviewType.OperatorReview,
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
                    Status = MechanicAcceptance.Status,
                    Type = ReviewType.MechanicAcceptance
                };
                Reviews.Add(review);
            }

            if (DispatcherReview != null)
            {
                var review = new ReviewDto()
                {
                    CheckPointId = Id,
                    Date = DispatcherReview.Date,
                    Notes = DispatcherReview.Notes,
                    ReviewerName = DispatcherReview.DispatcherName,
                    Status = ReviewStatus.Approved,
                    Type = ReviewType.DispatcherReview
                };
                Reviews.Add(review);
            }
        }
    }
}
