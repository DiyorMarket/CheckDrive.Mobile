using CheckDrive.Mobile.Models.Enums;

namespace CheckDrive.Mobile.Models.Driver
{
    public class DriverReviewResponse
    {
        public int CheckPointId { get; set; }
        public ReviewType ReviewType { get; set; }
        public bool IsAcceptedByDriver { get; set; }
        public string Notes { get; set; }
    }
}
