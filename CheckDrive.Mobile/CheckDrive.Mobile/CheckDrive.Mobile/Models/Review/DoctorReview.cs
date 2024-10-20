namespace CheckDrive.Mobile.Models.Review
{
    public class DoctorReview : ReviewBase
    {
        public int DriverId { get; private set; }

        public DoctorReview(int reviwerId, string notes, bool isApprovedByReviewer, int driverId)
            : base(reviwerId, notes, isApprovedByReviewer)
        {
            DriverId = driverId;
        }
    }
}
