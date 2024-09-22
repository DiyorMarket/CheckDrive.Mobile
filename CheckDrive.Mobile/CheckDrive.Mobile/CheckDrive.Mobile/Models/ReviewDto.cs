using CheckDrive.Mobile.Models.Enums;
using Syncfusion.XForms.ProgressBar;
using System;

namespace CheckDrive.Mobile.Models
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title => GetTitle(Type);
        public string Notes { get; set; }
        public string ReviewerName { get; set; }
        public DateTime Date { get; set; }
        public string Time => Date.ToString("HH:mm");
        public StepStatus StepStatus => GetStepStatus(Status);
        public int ProgressValue => GetProgressValue(Status);
        public ReviewType Type { get; set; }
        public ReviewStatus Status { get; set; }

        public static string GetTitle(ReviewType type)
        {
            switch (type)
            {
                case ReviewType.DoctorReview:
                    return "Shifokor tekshiruvi";
                case ReviewType.MechanicHandover:
                    return "Qabul qilish";
                case ReviewType.OperatorReview:
                    return "Yoqilg'i quyish";
                case ReviewType.MechanicAcceptance:
                    return "Topshirish";
                case ReviewType.DispatcherReview:
                    return "Dispetcher tekshiruvi";
                default:
                    return "Noma'lum";
            }
        }

        public static StepStatus GetStepStatus(ReviewStatus status)
        {
            switch (status)
            {
                case ReviewStatus.Approved:
                    return StepStatus.Completed;
                case ReviewStatus.InProgress:
                    return StepStatus.InProgress;
                default:
                    return StepStatus.NotStarted;
            }
        }

        public static int GetProgressValue(ReviewStatus status)
        {
            switch (status)
            {
                case ReviewStatus.Approved:
                    return 100;
                case ReviewStatus.InProgress:
                    return 50;
                default:
                    return 0;
            }
        }
    }
}
