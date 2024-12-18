using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Operator
{
    public class OperatorReview
    {
        public int CheckPointId { get; set; }
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public decimal InitialOilAmount { get; set; }
        public decimal OilRefillAmount { get; set; }
        public ReviewStatus Status { get; set; }
        public OilMark OilMark { get; set; }
    }
}
