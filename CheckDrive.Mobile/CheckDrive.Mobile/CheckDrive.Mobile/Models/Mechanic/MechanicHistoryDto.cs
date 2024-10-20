using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Mechanic
{
    public class MechanicHistoryDto
    {
        public int CheckPointId { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int CarId { get; set; }
        public string CarDetails { get; set; }
        public string Notes { get; set; }
        public ReviewStatus Status { get; set; }
        public DateTime Date { get; set; }
    }
}
