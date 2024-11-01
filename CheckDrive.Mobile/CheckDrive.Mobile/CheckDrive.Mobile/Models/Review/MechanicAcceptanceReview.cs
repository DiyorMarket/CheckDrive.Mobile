namespace CheckDrive.Mobile.Models.Review
{
    public class MechanicAcceptanceReview : ReviewBase
    {
        public int CheckPointId { get; set; }
        public int FinalMileage { get; set; }
        public decimal RemainingFuelAmount { get; set; }

        public MechanicAcceptanceReview()
            : base()
        {
        }

        public MechanicAcceptanceReview(int reviwerId, string notes, bool isApprovedByReviewer, int finalMileage, int checkPointId, decimal remainingFuelAmount)
            : base(reviwerId, notes, isApprovedByReviewer)
        {
            CheckPointId = checkPointId;
            FinalMileage = finalMileage;
            RemainingFuelAmount = remainingFuelAmount;
        }

        public override string GetReviewConfirmationMessage()
        {
            return $"Avtomobilni yakuniy {FinalMileage} km masofa va {RemainingFuelAmount} litr qoldiq yoqilg'i bilan topshirishni tasdiqlaysizmi?";
        }
    }
}
