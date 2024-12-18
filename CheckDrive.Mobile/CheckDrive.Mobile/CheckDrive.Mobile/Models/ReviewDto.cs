using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CheckDrive.Mobile.Models
{
    public class ReviewDto : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int CheckPointId { get; set; }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public string ReviewName => GetReviewName(Type);

        private string _reviewerName;
        public string ReviewerName
        {
            get => _reviewerName;
            set => SetProperty(ref _reviewerName, value);
        }

        public string FormattedReviewerName => string.IsNullOrWhiteSpace(ReviewerName)
            ? string.Empty
            : $"({ReviewerName})";

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
        public string Time => Date.ToString("HH:mm");
        public ReviewType Type { get; set; }

        private ReviewStatus _status;
        public ReviewStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public void Update(ReviewDto dto)
        {
            Id = dto.Id;
            Notes = dto.Notes;
            ReviewerName = dto.ReviewerName;
            Date = dto.Date;
            Status = dto.Status;
        }

        public void Update(ReviewStatus status)
        {
            Status = status;
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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            Action onChanged = null,
            [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
