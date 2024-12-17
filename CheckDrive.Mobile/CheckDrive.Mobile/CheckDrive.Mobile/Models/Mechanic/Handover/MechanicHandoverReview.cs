using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Mechanic.Handover
{
    public class MechanicHandoverReview
    {
        public int CheckPointId { get; set; }
        public int MechanicId { get; set; }
        public string MechanicName { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public int InitialMileage { get; set; }
        public ReviewStatus Status { get; set; }
        public CarDto Car { get; set; }
    }
}
