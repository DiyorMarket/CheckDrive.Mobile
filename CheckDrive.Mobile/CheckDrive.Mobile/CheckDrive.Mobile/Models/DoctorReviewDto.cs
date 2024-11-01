using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models
{
    public class DoctorReviewDto
    {
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public bool IsApproved { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public ReviewStatus Status { get; set; }
    }
}
