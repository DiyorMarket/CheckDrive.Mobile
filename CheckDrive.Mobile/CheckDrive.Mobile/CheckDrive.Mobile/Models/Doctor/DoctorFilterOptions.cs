using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckDrive.Mobile.Models.Doctor
{
    public class DoctorFilterOptions
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public List<PickerItem<int?>> Drivers { get; set; }
        public static List<PickerItem<ReviewStatus?>> Statuses => GetStatuses();
        public static List<PickerItem<string>> SortOptions => GetSortOptions();

        public DoctorFilterOptions(List<DoctorReview> histories)
        {
            Drivers = histories.Select(x => new PickerItem<int?>(x.DriverId, x.DriverName)).ToList();
            Drivers.Insert(0, new PickerItem<int?>(null, "Barcha haydovchilar"));

            MinDate = histories.Any()
                ? histories.Min(x => x.Date)
                : DateTime.Now;
            MaxDate = histories.Any()
                ? histories.Max(x => x.Date)
                : DateTime.Now;
        }

        private static List<PickerItem<ReviewStatus?>> GetStatuses() =>
            new List<PickerItem<ReviewStatus?>>
            {
                new PickerItem<ReviewStatus?>(null, "Barchasi"),
                new PickerItem<ReviewStatus?>(ReviewStatus.Approved, "Tasdiqlangan"),
                new PickerItem<ReviewStatus?>(ReviewStatus.Rejected, "Rad etilgan")
            };

        private static List<PickerItem<string>> GetSortOptions() =>
            new List<PickerItem<string>>()
            {
                new PickerItem<string>("date_desc", "Tekshiruv sanasi (tushish)"),
                new PickerItem<string>("date_asc", "Tekshiruv sanasi (o'sish)"),
                new PickerItem<string>("name_desc", "Haydovchilarni ismi (tushish)"),
                new PickerItem<string>("name_asc", "Haydovchilarni ismi (o'sish)")
            };
    }
}
