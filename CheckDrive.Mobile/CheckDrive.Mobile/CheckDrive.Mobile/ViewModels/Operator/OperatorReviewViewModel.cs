﻿using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Account;
using System.Collections.Generic;

namespace CheckDrive.Mobile.ViewModels.Operator
{
    public class OperatorReviewViewModel : BaseViewModel
    {
        public string FullName { get; }
        public List<OilMark> OilMarks { get; set; }
        public string Comments { get; set; }

        public OperatorReviewViewModel(AccountDto account, List<OilMark> oilMarks)
        {
            FullName = account.FirstName + " " + account.LastName;
            OilMarks = oilMarks;
        }
    }
}
