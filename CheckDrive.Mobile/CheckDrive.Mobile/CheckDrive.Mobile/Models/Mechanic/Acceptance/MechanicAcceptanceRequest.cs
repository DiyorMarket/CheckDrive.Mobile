namespace CheckDrive.Mobile.Models.Mechanic.Acceptance
{
    public class MechanicAcceptanceRequest
    {
        public int CheckPointId { get; set; }
        public int MechanicId { get; set; }
        public int FinalMileage { get; set; }
        public bool IsCarInGoodCondition { get; set; }
        public string Notes { get; set; }

        public MechanicAcceptanceRequest(int checkPointId, int mechanicId, int finalMileage, bool isCarInGoodCondition, string notes)
        {
            CheckPointId = checkPointId;
            MechanicId = mechanicId;
            FinalMileage = finalMileage;
            IsCarInGoodCondition = isCarInGoodCondition;
            Notes = notes;
        }
    }
}
