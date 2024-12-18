using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.ViewModels.Driver.Popups
{
    public class OperatorReviewConfirmationViewModel : BaseReviewConfirmationViewModel
    {
        public string OperatorName { get; }
        public decimal InitialOilAmount { get; }
        public decimal OilRefillAmount { get; }
        public int CheckPointId { get; }

        public OperatorReviewConfirmationViewModel(
            TaskCompletionSource<ReviewConfirmationRequest> completionSource,
            CheckPointDto checkPoint)
            : base(completionSource, checkPoint.Id, ReviewType.OperatorReview)
        {
            var review = checkPoint.OperatorReview ?? throw new InvalidOperationException("Cannot perform operator review confirmation without operator review.");

            OperatorName = review.OperatorName;
            InitialOilAmount = review.InitialOilAmount;
            OilRefillAmount = review.OilRefillAmount;
        }


    }
}
