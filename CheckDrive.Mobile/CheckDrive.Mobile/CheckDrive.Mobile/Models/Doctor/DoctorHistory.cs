using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Doctor
{
    public class DoctorHistory
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public ReviewStatus Status { get; set; }
        public bool IsApproved { get; set; }
    }
}
