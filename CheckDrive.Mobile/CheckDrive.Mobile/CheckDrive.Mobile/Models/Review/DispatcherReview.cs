namespace CheckDrive.Mobile.Models.Review
{
    public class DispatcherReview
    {
        public int CheckPointId { get; set; }
        public int ReviewerId { get; set; }
        public string Notes { get; set; }
        public bool IsApprovedByReviewer { get; set; }
        public decimal? FuelConsumptionAdjustment { get; set; }
        public int? DistanceTravelledAdjustment { get; set; }

        public DispatcherReview(int checkPointId, int reviewerId, string notes, bool isApprovedByReviewer, decimal? fuelConsumptionAdjustment, int? distanceTravelledAdjustment)
        {
            CheckPointId = checkPointId;
            ReviewerId = reviewerId;
            Notes = notes;
            IsApprovedByReviewer = isApprovedByReviewer;
            FuelConsumptionAdjustment = fuelConsumptionAdjustment;
            DistanceTravelledAdjustment = distanceTravelledAdjustment;
        }
    }
}
