namespace CheckDrive.Mobile.Models
{
    public class RideDetailDto
    {
        public decimal FuelConsumption { get; set; }
        public decimal? FuelConsumptionAdjustment { get; set; }
        public decimal DistanceTravelled { get; set; }
        public decimal? DistanceTravelledAdjustment { get; set; }
    }
}
