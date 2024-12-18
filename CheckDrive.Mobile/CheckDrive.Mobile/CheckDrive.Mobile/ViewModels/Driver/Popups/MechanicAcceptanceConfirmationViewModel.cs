using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.ViewModels.Driver.Popups
{
    public class MechanicAcceptanceConfirmationViewModel : BaseReviewConfirmationViewModel
    {
        public string MechanicName { get; }
        public int FinalMileage { get; set; }
        public string CarState { get; set; }

        public MechanicAcceptanceConfirmationViewModel(
            TaskCompletionSource<ReviewConfirmationRequest> completionSource,
            CheckPointDto checkPoint)
            : base(completionSource, checkPoint.Id, ReviewType.MechanicAcceptance)
        {
            var review = checkPoint.MechanicAcceptance ?? throw new InvalidOperationException("Cannot perform mechanic acceptance without mechanic acceptance.");

            MechanicName = review.MechanicName;
            FinalMileage = review.FinalMileage;
            CarState = review.IsCarInGoodCondition ? "Soz" : "Nosoz";
        }
    }
}
