using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Review
{
    public class MechanicAcceptanceDto
    {
        public int CheckPointId { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public ReviewStatus Status { get; set; }
        public int FinalMileage { get; set; }
        public decimal RemainingFuelAmount { get; set; }
    }
}
