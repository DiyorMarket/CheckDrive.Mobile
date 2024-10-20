using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Mechanic
{
    public class MechanicFilter
    {
        public int? DriverId { get; set; }
        public int? CarId { get; set; }
        public ReviewStatus? Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SortBy { get; set; } = "date_desc";
    }
}
