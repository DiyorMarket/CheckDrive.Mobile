using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Views.Driver.Popups;
using Rg.Plugins.Popup.Pages;
using System;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Services
{
    public static class ReviewConfirmationFactory
    {
        public static PopupPage GetConfirmationPopup(
            TaskCompletionSource<ReviewConfirmationRequest> completionSource,
            CheckPointDto checkPoint,
            ReviewType reviewType)
        {
            switch (reviewType)
            {
                case ReviewType.MechanicHandover:
                    return new MechanicHandoverConfirmationPopup(completionSource, checkPoint);
                case ReviewType.OperatorReview:
                    return new OperatorReviewConfirmationPopup(completionSource, checkPoint);
                case ReviewType.MechanicAcceptance:
                    return new MechanicAcceptanceConfirmationPopup(completionSource, checkPoint);
                default:
                    throw new ArgumentOutOfRangeException(nameof(reviewType));
            }
        }
    }
}
