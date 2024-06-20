using CheckDrive.ApiContracts.DoctorReview;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckDrive.Web.Stores.DoctorReviews
{
    public class MockDoctorReviewDataStore 
    {
        private readonly List<DoctorReviewDto> _reviews;

        public MockDoctorReviewDataStore()
        {
            _reviews = new List<DoctorReviewDto>
            {
                new DoctorReviewDto { Id = 1, IsHealthy = true, Comments = "Good condition", Date = DateTime.Now, DriverId = 1, DriverName = "Jose" },
                new DoctorReviewDto { Id = 9, IsHealthy = true, Comments = "Good condition", Date = DateTime.Now, DriverId = 1, DriverName = "Ravshan" },
                new DoctorReviewDto { Id = 2, IsHealthy = false, Comments = "Needs rest", Date = DateTime.Now.AddDays(-1), DriverId = 2, DriverName = "Willian" },
                new DoctorReviewDto { Id = 3, IsHealthy = true, Comments = "Good condition", Date = DateTime.Now.AddDays(-2), DriverId = 1, DriverName = "Jose" },
                new DoctorReviewDto { Id = 4, IsHealthy = false, Comments = "Needs rest", Date = DateTime.Now.AddDays(-2), DriverId = 2, DriverName = "Willian" },
                new DoctorReviewDto { Id = 5, IsHealthy = true, Comments = "Good condition", Date = DateTime.Now.AddDays(-3), DriverId = 1, DriverName = "Jose" },
                new DoctorReviewDto { Id = 6, IsHealthy = false, Comments = "Needs rest", Date = DateTime.Now.AddDays(-4), DriverId = 2, DriverName = "Willian" },
                new DoctorReviewDto { Id = 7, IsHealthy = true, Comments = "Good condition", Date = DateTime.Now.AddDays(-5), DriverId = 1, DriverName = "Jose" },
                new DoctorReviewDto { Id = 8, IsHealthy = false, Comments = "Needs rest", Date = DateTime.Now.AddDays(-5), DriverId = 2, DriverName = "Willian" },
            };
        }

        public List<DoctorReviewDto> GetDoctorReviews()
        {
            return _reviews.ToList();
        }

        public DoctorReviewDto GetDoctorReview(int id)
        {
            return _reviews.FirstOrDefault(r => r.Id == id);
        }

        public DoctorReviewDto CreateDoctorReview(DoctorReviewForCreateDto review)
        {
            var newDoctorReview = new DoctorReviewDto
            {
                Id = _reviews.Count + 1,
                IsHealthy = review.IsHealthy,
                Comments = review.Comments,
                Date = review.Date,
                DriverId = review.DriverId
            };

            _reviews.Add(newDoctorReview);
            return newDoctorReview;
        }

        public DoctorReviewDto UpdateDoctorReview(int id, DoctorReviewForUpdateDto review)
        {
            var existingReview = _reviews.FirstOrDefault(r => r.Id == id);
            if (existingReview != null)
            {
                existingReview.IsHealthy = review.IsHealthy;
                existingReview.Comments = review.Comments;
                existingReview.Date = review.Date;
                existingReview.DriverId = review.DriverId;
            }
            return existingReview;
        }

        public void DeleteDoctorReview(int id)
        {
            var reviewToRemove = _reviews.FirstOrDefault(r => r.Id == id);
            if (reviewToRemove != null)
            {
                _reviews.Remove(reviewToRemove);
            }
        }
    }
}
