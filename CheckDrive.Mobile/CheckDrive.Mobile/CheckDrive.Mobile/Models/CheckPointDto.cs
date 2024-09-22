using CheckDrive.Mobile.Models.Enums;
using System.Collections.Generic;

namespace CheckDrive.Mobile.Models
{
    public class CheckPointDto
    {
        public CheckPointStage Stage { get; set; }
        public CheckPointStatus Status { get; set; }
        public CarDto Car { get; set; }

        public List<ReviewDto> Reviews { get; set; }
    }
}
