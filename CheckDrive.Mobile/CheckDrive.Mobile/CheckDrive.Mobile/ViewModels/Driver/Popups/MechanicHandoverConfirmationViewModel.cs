using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.ViewModels.Driver.Popups
{
    public class MechanicHandoverConfirmationViewModel : BaseReviewConfirmationViewModel
    {
        public string MechanicName { get; }
        public string Car { get; }
        public int InitialMileage { get; }

        public MechanicHandoverConfirmationViewModel(
            TaskCompletionSource<ReviewConfirmationRequest> completionSource,
            CheckPointDto checkPoint)
            : base(completionSource, checkPoint.Id, ReviewType.MechanicHandover)
        {
            var review = checkPoint.MechanicHandover ?? throw new InvalidOperationException("Cannot perform mechanic handover confirmation without mechanic handover.");
            var car = checkPoint.Car ?? throw new InvalidOperationException("Cannot perform mechanic handover confirmation without Car.");

            MechanicName = review.MechanicName;
            InitialMileage = review.InitialMileage;
            Car = $"{car.Model} {car.Number}";
        }
    }
}
