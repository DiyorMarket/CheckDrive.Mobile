using System;

namespace CheckDrive.Mobile.Models.Dispatcher
{
    public class DispatcherHistory
    {
        public int CheckPointId { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public int FinalMileage { get; set; }
        public decimal FuelConsumptionAmount { get; set; }
        public decimal RemainingFuelAmount { get; set; }
    }
}
