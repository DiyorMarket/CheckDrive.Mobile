using System;
using System.Globalization;

namespace CheckDrive.Mobile.Helpers
{
    public static class DateHelper
    {
        public static string ConvertDateTimeToDate(string dateTimeString, String langCulture)
        {
            CultureInfo culture = new CultureInfo(langCulture);

            if (DateTime.TryParse(dateTimeString, culture, DateTimeStyles.AssumeLocal, out var date))
            {
                return date.ToString("d", culture);
            }

            return dateTimeString;
        }
    }
}
