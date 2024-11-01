namespace CheckDrive.Mobile.Models.Review
{
    public class DispatcherReviewDto
    {
        public int CheckPointId { get; set; }
        public int ReviewerId { get; set; }
        public string Notes { get; set; }
        public bool IsApprovedByReviewer { get; set; }
        public decimal? FuelConsumptionAdjustment { get; set; }
        public int? DistanceTravelledAdjustment { get; set; }
    }
}
