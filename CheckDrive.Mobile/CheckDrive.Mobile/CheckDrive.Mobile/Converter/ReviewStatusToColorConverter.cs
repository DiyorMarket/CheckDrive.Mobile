using CheckDrive.Mobile.Models.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Converter
{
    public class ReviewStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReviewStatus status)
            {
                switch (status)
                {
                    case ReviewStatus.InProgress:
                        return Color.FromHex("#04aa6d");
                    case ReviewStatus.Approved:
                        return Color.FromHex("#04aa6d");
                    case ReviewStatus.RejectedByReviewer:
                    case ReviewStatus.RejectedByDriver:
                        return Color.FromHex("#bd3e3e");
                    case ReviewStatus.NotStarted:
                        return Color.FromHex("#1f2121");
                    default:
                        return Color.Black;
                }
            }

            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
