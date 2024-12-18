namespace CheckDrive.Mobile.Models.Dispatcher
{
    public class DispatcherReviewRequest
    {
        public int CheckPointId { get; set; }
        public int DispatcherId { get; set; }
        public string Notes { get; set; }
        public int FinalMileage { get; set; }
        public decimal FuelConsumptionAmount { get; set; }
        public decimal RemainingFuelAmount { get; set; }

        public DispatcherReviewRequest(
            int checkPointId,
            int dispatcherId,
            string notes,
            int finalMileage,
            decimal fuelConsumptionAmount,
            decimal remainingFuelAmount)
        {
            CheckPointId = checkPointId;
            DispatcherId = dispatcherId;
            Notes = notes;
            FinalMileage = finalMileage;
            FuelConsumptionAmount = fuelConsumptionAmount;
            RemainingFuelAmount = remainingFuelAmount;
        }
    }
}
