using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Review
{
    public class MockReviewStore : IReviewStore
    {
        public Task<CheckPointDto> GetCheckPointAsync()
        {
            var reviews = GetMockReviews();
            var checkPoint = new CheckPointDto
            {
                Status = CheckPointStatus.InProcess,
                Stage = CheckPointStage.DoctorReview,
                Reviews = reviews,
            };

            return Task.FromResult(checkPoint);
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
                StartDate = DateTime.Now,
                Notes = "Doctor Review.",
                Status = ReviewStatus.Approved,
                Type = ReviewType.DoctorReview
            });
            reviews.Add(new ReviewDto
            {
                Id = 1,
                StartDate = DateTime.Now,
                Notes = "Mechanic Handover.",
                Status = ReviewStatus.Approved,
                Type = ReviewType.MechanicHandover
            });
            reviews.Add(new ReviewDto
            {
                Id = 1,
                StartDate = DateTime.Now,
                Notes = "Operator Review.",
                Status = ReviewStatus.InProgress,
                Type = ReviewType.OperatorReview
            });
            reviews.Add(new ReviewDto
            {
                Id = 1,
                StartDate = DateTime.Now,
                Notes = "Mechanic Review.",
                Status = ReviewStatus.NotStarted,
                Type = ReviewType.MechanicAcceptance
            });
            reviews.Add(new ReviewDto
            {
                Id = 1,
                StartDate = DateTime.Now,
                Notes = "Dispatcher Review.",
                Status = ReviewStatus.NotStarted,
                Type = ReviewType.DispatcherReview
            });

            return reviews;
        }
    }
}
