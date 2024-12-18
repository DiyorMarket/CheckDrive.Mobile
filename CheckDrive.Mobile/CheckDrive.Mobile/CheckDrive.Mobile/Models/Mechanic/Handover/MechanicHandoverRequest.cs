namespace CheckDrive.Mobile.Models.Mechanic.Handover
{
    public class MechanicHandoverRequest
    {
        public int CheckPointId { get; set; }
        public int MechanicId { get; set; }
        public int CarId { get; set; }
        public int InitialMileage { get; set; }
        public string Notes { get; set; }

        public MechanicHandoverRequest()
        {
        }

        public MechanicHandoverRequest(int checkPointId, int mechanicId, int carId, int initialMileage, string notes)
        {
            CheckPointId = checkPointId;
            MechanicId = mechanicId;
            CarId = carId;
            InitialMileage = initialMileage;
            Notes = notes;
        }
    }
}
