namespace CheckDrive.Mobile.Models.Review
{
    public class MechanicAcceptanceReview : ReviewBase
    {
        public int CheckPointId { get; set; }
        public int FinalMileage { get; set; }

        public MechanicAcceptanceReview(int reviwerId, string notes, bool isApprovedByReviewer, int finalMileage, int checkPointId)
            : base(reviwerId, notes, isApprovedByReviewer)
        {
            FinalMileage = finalMileage;
            CheckPointId = checkPointId;
        }
    }
}
