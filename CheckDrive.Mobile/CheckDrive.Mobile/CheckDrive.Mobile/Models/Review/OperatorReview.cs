namespace CheckDrive.Mobile.Models.Review
{
    public class OperatorReview : ReviewBase
    {
        public int DriverId { get; set; }

        public OperatorReview(int reviwerId, string notes, bool isApprovedByReviewer, int driverId)
            : base(reviwerId, notes, isApprovedByReviewer)
        {
            DriverId = driverId;
        }
    }
}
