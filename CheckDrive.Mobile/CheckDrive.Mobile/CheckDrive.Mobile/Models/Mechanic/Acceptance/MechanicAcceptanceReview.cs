using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Mechanic.Acceptance
{
    public class MechanicAcceptanceReview
    {
        public int CheckPointId { get; set; }
        public int MechanicId { get; set; }
        public string MechanicName { get; set; }
        public int FinalMileage { get; set; }
        public bool IsCarInGoodCondition { get; set; }
        public ReviewStatus Status { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
    }
}
