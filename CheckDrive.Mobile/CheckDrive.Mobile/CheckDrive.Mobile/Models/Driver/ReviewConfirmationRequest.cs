using CheckDrive.Mobile.Models.Enums;

namespace CheckDrive.Mobile.Models.Driver
{
    public class ReviewConfirmationRequest
    {
        public int CheckPointId { get; }
        public ReviewType ReviewType { get; }
        public bool IsAccepted { get; }
        public string Notes { get; }

        public ReviewConfirmationRequest(int checkPointId, ReviewType reviewType, bool isAcceptedByDriver, string notes)
        {
            CheckPointId = checkPointId;
            ReviewType = reviewType;
            IsAccepted = isAcceptedByDriver;
            Notes = notes;
        }
    }
}
