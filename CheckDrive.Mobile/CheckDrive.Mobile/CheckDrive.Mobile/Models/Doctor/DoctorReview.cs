using System;

namespace CheckDrive.Mobile.Models.Doctor
{
    public class DoctorReview
    {
        public int CheckPointId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public bool IsHealthy { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
    }
}
