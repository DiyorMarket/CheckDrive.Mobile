using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Doctor
{
    public class DoctorFilter
    {
        public int? SelectedDriverId { get; set; }
        public ReviewStatus? SelectedStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SortBy { get; set; } = "date_desc";
    }
}
