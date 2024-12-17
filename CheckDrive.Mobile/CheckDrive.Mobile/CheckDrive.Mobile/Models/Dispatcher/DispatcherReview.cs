using System;

namespace CheckDrive.Mobile.Models.Dispatcher
{
    public class DispatcherReview
    {
        public int CheckPointId { get; set; }
        public int DispatcherId { get; set; }
        public string DispatcherName { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public int FinalMileage { get; set; }
        public decimal FuelConsumptionAmount { get; set; }
        public decimal RemainingFuelAmount { get; set; }
    }
}
