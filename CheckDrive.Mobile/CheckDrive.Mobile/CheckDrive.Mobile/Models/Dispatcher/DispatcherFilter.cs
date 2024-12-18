using System;

namespace CheckDrive.Mobile.Models.Dispatcher
{
    public class DispatcherFilter
    {
        public int? SelectedDriverId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SortBy { get; set; }

        public DispatcherFilter(
            int? selectedDriverId,
            DateTime startDate,
            DateTime endDate,
            string sortBy = "date_desc")
        {
            SelectedDriverId = selectedDriverId;
            StartDate = startDate;
            EndDate = endDate;
            SortBy = sortBy;
        }

        public static DispatcherFilter GetDefaultFilter(DateTime minDate, DateTime maxDate)
            => new DispatcherFilter(null, minDate, maxDate);
    }
}
