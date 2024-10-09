using CheckDrive.Mobile.Models;
using System.Collections.Generic;

namespace CheckDrive.Mobile.ViewModels.Operator
{
    public class OperatorReviewViewModel : BaseViewModel
    {
        public string FullName { get; }
        public List<OilMark> OilMarks { get; set; }
        public string Comments { get; set; }

        public OperatorReviewViewModel(DriverDto driver, List<OilMark> oilMarks)
        {
            FullName = driver.FullName;
            OilMarks = oilMarks;
        }
    }
}
