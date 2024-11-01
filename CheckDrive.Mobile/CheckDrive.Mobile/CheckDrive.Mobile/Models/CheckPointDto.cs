using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Review;
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
        public DoctorReviewDto DoctorReview { get; set; }
        public MechanicHandoverReviewDto MechanicHandover { get; set; }
        public OperatorReviewDto OperatorReview { get; set; }
        public MechanicAcceptanceDto MechanicAcceptance { get; set; }

        public string DriverName => DoctorReview?.DriverName;
        public CarDto Car => MechanicHandover?.Car;
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
                    ReviewerName = MechanicHandover.ReviewerName,
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
                    ReviewerName = OperatorReview.ReviewerName,
                    Status = OperatorReview.Status,
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
                    ReviewerName = MechanicAcceptance.ReviewerName,
                    Status = MechanicAcceptance.Status,
                    Type = ReviewType.MechanicAcceptance
                };
                Reviews.Add(review);
            }
        }
    }
}
