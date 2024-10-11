using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;

namespace CheckDrive.Mobile.Models
{
    public class CheckPointDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public CheckPointStage Stage { get; set; }
        public CheckPointStatus Status { get; set; }
        public DoctorReviewDto DoctorReview { get; set; }
        public MechanicHandoverReviewDto MechanicHandover { get; set; }

        public string DriverName => DoctorReview.DriverName;
        public CarDto Car { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
