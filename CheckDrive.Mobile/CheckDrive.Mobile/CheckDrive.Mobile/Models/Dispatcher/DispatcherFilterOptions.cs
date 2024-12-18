using CheckDrive.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckDrive.Mobile.Models.Dispatcher
{
    public class DispatcherFilterOptions
    {
        public List<PickerItem<int?>> Drivers { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public static List<PickerItem<string>> SortOptions => new List<PickerItem<string>>
        {
            new PickerItem<string>("date_desc", "Tekshiruv sanasi (tushish)"),
            new PickerItem<string>("date_asc", "Tekshiruv sanasi (o'sish)"),
            new PickerItem<string>("name_desc", "Haydovchilarni ismi (tushish)"),
            new PickerItem<string>("name_asc", "Haydovchilarni ismi (o'sish)")
        };

        public DispatcherFilterOptions(List<DispatcherHistory> histories)
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
    }
}
