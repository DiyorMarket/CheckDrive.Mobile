using CheckDrive.Mobile.Models.Enums;

namespace CheckDrive.Mobile.Models
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public int ManufacturedYear { get; set; }
        public int Mileage { get; set; }
        public int YearlyDistanceLimit { get; set; }
        public decimal AverageFuelConsumption { get; set; }
        public decimal FuelCapacity { get; set; }
        public decimal RemainingFuel { get; set; }
        public decimal MonthlyDistanceLimit { get; set; }
        public decimal CurrentMonthMileage { get; set; }
        public CarStatus Status { get; set; }

        public override string ToString()
        {
            return $"{Color} {Model} ({Number})";
        }
    }
}
