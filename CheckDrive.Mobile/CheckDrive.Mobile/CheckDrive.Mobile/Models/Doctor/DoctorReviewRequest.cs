namespace CheckDrive.Mobile.Models.Doctor
{
    public class DoctorReviewRequest
    {
        public int DriverId { get; set; }
        public int DoctorId { get; set; }
        public string Notes { get; set; }
        public bool IsHealthy { get; set; }

        public DoctorReviewRequest(int driverId, int doctorId, bool isHealthy, string notes)
        {
            DriverId = driverId;
            DoctorId = doctorId;
            IsHealthy = isHealthy;
            Notes = notes;
        }
    }
}
