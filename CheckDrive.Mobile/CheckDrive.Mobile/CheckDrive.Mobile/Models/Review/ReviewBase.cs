namespace CheckDrive.Mobile.Models.Review
{
    public abstract class ReviewBase
    {
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public string Notes { get; set; }
        public bool IsApprovedByReviewer { get; set; }

        protected ReviewBase()
        {
        }

        protected ReviewBase(int reviewerId, string notes, bool isApprovedByReviewer)
        {
            ReviewerId = reviewerId;
            Notes = notes;
            IsApprovedByReviewer = isApprovedByReviewer;
        }

        public abstract string GetReviewConfirmationMessage();
    }
}
