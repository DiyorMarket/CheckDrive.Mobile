using System;

namespace CheckDrive.Mobile.Models.Review
{
    public class OperatorReview : ReviewBase
    {
        public int DriverId { get; set; }
        public string FullName { get; set; }
        public OilMark SelectedOilMark { get; set; }
        public string InitialOilAmount { get; set; }
        public string OilRefilAmount { get; set; }
        public string Comments { get; set; }

        public OperatorReview(int reviwerId, string notes, bool isApprovedByReviewer, int driverId, string fullname, OilMark selectedOilMark, string initialOilAmount, string oilRefilAmount, string comments)
            : base(reviwerId, notes, isApprovedByReviewer)
        {
            DriverId = driverId;
            FullName = fullname;
            SelectedOilMark = selectedOilMark ?? throw new ArgumentNullException(nameof(selectedOilMark));
            InitialOilAmount = initialOilAmount ?? throw new ArgumentNullException(nameof(initialOilAmount));
            OilRefilAmount = oilRefilAmount ?? throw new ArgumentNullException(nameof(oilRefilAmount));
            Comments = comments;    
        }
    }
}
