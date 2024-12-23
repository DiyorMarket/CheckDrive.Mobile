using CheckDrive.Mobile.Models.Enums;
using System;

namespace CheckDrive.Mobile.Models.Driver
{
    public class DriverHistory
    {
        public int DriverId { get; set; }
        public int TravelledDistance { get; set; }
        public decimal FuelConsumptionAmount { get; set; }
        public decimal DebtAmount { get; set; }
        public CheckPointStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public CarDto Car { get; set; }

        public string CarName => $"{Car.Model} ({Car.Number})";
        public bool HasDebt => DebtAmount > 0;
    }
}
