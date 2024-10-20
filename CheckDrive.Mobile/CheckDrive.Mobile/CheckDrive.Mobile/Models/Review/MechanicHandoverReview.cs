namespace CheckDrive.Mobile.Models.Review
{
    public class MechanicHandoverReview : ReviewBase
    {
        public int CheckPointId { get; set; }
        public int CarId { get; set; }
        public int InitialMileage { get; set; }
        public CarDto Car { get; set; }

        public MechanicHandoverReview(int reviewerId, string notes, bool isApprovedByReviewer, int carId, int initialMileage, int checkPointId, CarDto car)
            : base(reviewerId, notes, isApprovedByReviewer)
        {
            CarId = carId;
            InitialMileage = initialMileage;
            CheckPointId = checkPointId;
            Car = car;
        }
    }
}
