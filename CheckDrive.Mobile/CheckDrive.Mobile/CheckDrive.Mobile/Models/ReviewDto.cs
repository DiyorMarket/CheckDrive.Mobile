using CheckDrive.Mobile.Models.Enums;
using Syncfusion.XForms.ProgressBar;
using System;

namespace CheckDrive.Mobile.Models
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public string ReviewName => GetReviewName(Type);
        public string ReviewerName { get; set; }
        public string FormattedReviewerName => string.IsNullOrWhiteSpace(ReviewerName)
            ? string.Empty
            : $"({ReviewerName})";
        public DateTime Date { get; set; }
        public string Time => Date.ToString("HH:mm");
        public StepStatus StepStatus => GetStepStatus(Status);
        public int ProgressValue => GetProgressValue(Status);
        public ReviewType Type { get; set; }
        public ReviewStatus Status { get; set; }

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

        public static string GetReviewName(ReviewType type)
        {
            switch (type)
            {
                case ReviewType.DoctorReview:
                    return "Shifokor ko'rigi";
                case ReviewType.MechanicHandover:
                    return "Avtomobil qabul qilish";
                case ReviewType.OperatorReview:
                    return "Yoqilg'i quyish";
                case ReviewType.MechanicAcceptance:
                    return "Avtomobil topshirish";
                case ReviewType.DispatcherReview:
                    return "Dispetcher tekshiruvi";
                default:
                    return "";
            }
        }
    }
}
