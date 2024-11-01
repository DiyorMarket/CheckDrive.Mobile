namespace CheckDrive.Mobile.Models.Review
{
    public class OperatorReview : ReviewBase
    {
        public int CheckPointId { get; set; }
        public int OilMarkId { get; set; }
        public string OilMarkName { get; set; }
        public decimal InitialOilAmount { get; set; }
        public decimal OilRefillAmount { get; set; }

        public OperatorReview()
            : base()
        {
        }

        public OperatorReview(
            int reviewerId,
            string notes,
            bool isApprovedByReviewer,
            int checkPointId,
            int oilMarkId,
            string oilMarkName,
            int initialOilAmount,
            int oilRefillAmount)
            : base(reviewerId, notes, isApprovedByReviewer)
        {
            CheckPointId = checkPointId;
            OilMarkId = oilMarkId;
            OilMarkName = oilMarkName;
            InitialOilAmount = initialOilAmount;
            OilRefillAmount = oilRefillAmount;
        }

        public override string GetReviewConfirmationMessage()
        {
            return $"{OilRefillAmount} litr ({OilMarkName}) yoqilg'i {ReviewerName}dan qabul qilishni tasdiqlaysizmi?";
        }
    }
}
