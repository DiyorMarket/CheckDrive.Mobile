using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Operator
{
    public class OperatorFilter
    {
        public int? DriverId { get; set; }
        public int? OilMarkId { get; set; }
        public ReviewStatus? Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SortBy { get; set; } = "date_desc";

        public static OperatorFilter GetDefaultFilters(DateTime startDate, DateTime endDate)
            => new OperatorFilter
            {
                DriverId = null,
                OilMarkId = null,
                Status = null,
                StartDate = startDate,
                EndDate = endDate,
            };
    }
}
