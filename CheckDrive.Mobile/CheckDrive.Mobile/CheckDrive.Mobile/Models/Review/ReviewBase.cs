namespace CheckDrive.Mobile.Models.Review
{
    public abstract class ReviewBase
    {
        public int ReviwerId { get; protected set; }
        public string Notes { get; protected set; }
        public bool IsApprovedByReviewer { get; protected set; }

        protected ReviewBase(int reviwerId, string notes, bool isApprovedByReviewer)
        {
            ReviwerId = reviwerId;
            Notes = notes;
            IsApprovedByReviewer = isApprovedByReviewer;
        }
    }
}
