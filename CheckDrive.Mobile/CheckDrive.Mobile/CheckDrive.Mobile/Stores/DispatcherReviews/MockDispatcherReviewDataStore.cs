using CheckDrive.ApiContracts.DispatcherReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckDrive.Web.Stores.DispatcherReviews
{
    public class MockDispatcherReviewDataStore
    {
        private readonly List<DispatcherReviewDto> _reviews;

        public MockDispatcherReviewDataStore()
        {
            _reviews = new List<DispatcherReviewDto>
            {
                new DispatcherReviewDto { Id = 1, FuelSpended = 50, DistanceCovered = 100, Date = DateTime.Now, DriverId = 1, DriverName = "John", DispatcherId = 1, DispatcherName = "Harry1", MechanicId = 1, MechanicName = "Bill1", OperatorId = 1, OperatorName = "Jack1"},
                new DispatcherReviewDto { Id = 2, FuelSpended = 45, DistanceCovered = 120, Date = DateTime.Now, DriverId = 1, DriverName = "John", DispatcherId = 2, DispatcherName = "Harry2", MechanicId = 2, MechanicName = "Bill2", OperatorId = 2, OperatorName = "Jack2"},
                new DispatcherReviewDto { Id = 3, FuelSpended = 40, DistanceCovered = 80, Date = DateTime.Now, DriverId = 1, DriverName = "John", DispatcherId = 2, DispatcherName = "Harry2", MechanicId = 1, MechanicName = "Bill1", OperatorId = 3, OperatorName = "Jack3"},
                new DispatcherReviewDto { Id = 4, FuelSpended = 55, DistanceCovered = 90, Date = DateTime.Now, DriverId = 4, DriverName = "John4", DispatcherId = 3, DispatcherName = "Harry3", MechanicId = 3, MechanicName = "Bill3", OperatorId = 4, OperatorName = "Jack4"},
                new DispatcherReviewDto { Id = 5, FuelSpended = 60, DistanceCovered = 50, Date = DateTime.Now, DriverId = 3, DriverName = "John3", DispatcherId = 3, DispatcherName = "Harry3", MechanicId = 1, MechanicName = "Bill1", OperatorId = 5, OperatorName = "Jack5"},
            };
        }

        public async Task<List<DispatcherReviewDto>> GetDispatcherReviewsAsync()
        {
            await Task.Delay(100);
            return _reviews ;
        }

        public DispatcherReviewDto GetDispatcherReview(int id)
        {
            return _reviews.FirstOrDefault(r => r.Id == id);
        }

        public DispatcherReviewDto CreateDispatcherReview(DispatcherReviewForCreateDto dispatcherReview)
        {
            var newDispatcherReview = new DispatcherReviewDto
            {
                Id = _reviews.Count + 1,
                DispatcherId = dispatcherReview.DispatcherId,
                Date = dispatcherReview.Date,
                DriverId = dispatcherReview.DriverId,
                FuelSpended = dispatcherReview.FuelSpended,
                DistanceCovered = dispatcherReview.DistanceCovered,
                MechanicId = dispatcherReview.MechanicId,
                OperatorId = dispatcherReview.OperatorId
            };
            _reviews.Add(newDispatcherReview);
            return newDispatcherReview;
        }

        public DispatcherReviewDto UpdateDispatcherReview(int id, DispatcherReviewForUpdateDto dispatcherReview)
        {
            var existingReview = _reviews.FirstOrDefault(r => r.Id == id);
            if (existingReview != null)
            {
                existingReview.FuelSpended = dispatcherReview.FuelSpended;
                existingReview.Date = dispatcherReview.Date;
                existingReview.DriverId = dispatcherReview.DriverId;
                existingReview.MechanicId = dispatcherReview.MechanicId;
                existingReview.OperatorId = dispatcherReview.OperatorId;
                existingReview.DistanceCovered = dispatcherReview.DistanceCovered;
                existingReview.Date = dispatcherReview.Date;
            }
            return existingReview;
        }

        public void DeleteDispatcherReview(int id)
        {
            var reviewToRemove = _reviews.FirstOrDefault(r => r.Id == id);
            if (reviewToRemove != null)
            {
                _reviews.Remove(reviewToRemove);
            }
        }

    }
}
