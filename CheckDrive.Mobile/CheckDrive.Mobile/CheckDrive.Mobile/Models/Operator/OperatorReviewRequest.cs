namespace CheckDrive.Mobile.Models.Operator
{
    public class OperatorReviewRequest
    {
        public int CheckPointId { get; set; }
        public int OperatorId { get; set; }
        public int OilMarkId { get; set; }
        public string Notes { get; set; }
        public decimal InitialOilAmount { get; set; }
        public decimal OilRefillAmount { get; set; }

        public OperatorReviewRequest(
            int checkPointId,
            int operatorId,
            int oilMarkId,
            string notes,
            decimal initialOilAmount,
            decimal oilRefillAmount)
        {
            CheckPointId = checkPointId;
            OperatorId = operatorId;
            OilMarkId = oilMarkId;
            Notes = notes;
            InitialOilAmount = initialOilAmount;
            OilRefillAmount = oilRefillAmount;
        }
    }
}
