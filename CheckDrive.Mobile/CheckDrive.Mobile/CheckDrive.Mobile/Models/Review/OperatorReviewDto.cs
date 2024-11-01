using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Review
{
    public class OperatorReviewDto
    {
        public int CheckPointId { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public string OilMarkName { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public ReviewStatus Status { get; set; }
        public decimal InitialOilAmount { get; set; }
        public decimal OilRefillAmount { get; set; }
    }
}
