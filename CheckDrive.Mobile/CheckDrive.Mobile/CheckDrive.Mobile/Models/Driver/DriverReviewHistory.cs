using CheckDrive.Mobile.Models.Enums;
using System;
using System.Collections.Generic;

namespace CheckDrive.Mobile.Models.Driver
{
    public class DriverReviewHistory
    {
        public string IconSource { get; set; }
        public string CarDetails { get; set; }
        public string RideDetails { get; set; }
        public DateTime Date { get; set; }


        public DateTime StartDate { get; set; }
        public CheckPointStatus Status { get; set; }
        public CarDto Car { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
