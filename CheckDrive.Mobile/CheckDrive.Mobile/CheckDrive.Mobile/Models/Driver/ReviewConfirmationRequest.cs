namespace CheckDrive.Mobile.Models.Driver
{
    public class ReviewConfirmationRequest
    {
        public int CheckPointId { get; set; }
        public int ReviewerId { get; set; }
        public string Message { get; set; }
    }
}
