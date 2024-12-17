using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckDrive.Mobile.Models.Operator
{
    public class OperatorFilterOptions
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public List<PickerItem<int?>> Drivers { get; set; }
        public List<PickerItem<int?>> OilMarks { get; set; }
        public static List<PickerItem<ReviewStatus?>> Statuses => GetStatuses();
        public static List<PickerItem<string>> SortOptions => GetSortOptions();

        public OperatorFilterOptions(List<OperatorHistoryDto> histories)
        {
            Drivers = histories.Select(x => new PickerItem<int?>(x.DriverId, x.DriverName)).Distinct().ToList();
            Drivers.Insert(0, new PickerItem<int?>(null, "Barcha haydovchilar"));

            OilMarks = histories.Select(x => new PickerItem<int?>(x.OilMarkId, x.OilMarkName)).Distinct().ToList();
            OilMarks.Insert(0, new PickerItem<int?>(null, "Barcha markalar"));

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
