using System;

namespace CheckDrive.Mobile.Models.Operator
{
    public class OperatorHistoryDto
    {
        public int CheckPointId { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int OilMarkId { get; set; }
        public string OilMarkName { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public decimal InitialOilAmount { get; set; }
        public decimal OilRefillAmount { get; set; }
    }
}
