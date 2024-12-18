using System;

namespace CheckDrive.Mobile.Models.Doctor
{
    public class DoctorFilter
    {
        public int? SelectedDriverId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SortBy { get; set; } = "date_desc";

        public static DoctorFilter GetDefaultFilter(DateTime minDate, DateTime maxDate) =>
            new DoctorFilter()
            {
                SelectedDriverId = null,
                SortBy = "date_desc",
                StartDate = minDate,
                EndDate = maxDate,
            };
    }
}
