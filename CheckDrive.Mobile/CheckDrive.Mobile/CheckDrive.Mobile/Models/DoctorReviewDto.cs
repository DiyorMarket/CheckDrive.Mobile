namespace CheckDrive.Mobile.Models
{
    public class DoctorReviewDto
    {
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public bool IsApproved { get; set; }
    }
}
