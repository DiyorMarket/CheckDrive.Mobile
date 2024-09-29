using CheckDrive.Mobile.Models.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Converter
{
    public class ReviewStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReviewStatus status)
            {
                switch (status)
                {
                    case ReviewStatus.InProgress:
                        return "";
                    case ReviewStatus.Approved:
                        return "icon_success.png";
                    case ReviewStatus.RejectedByReviewer:
                    case ReviewStatus.RejectedByDriver:
                        return "icon_error.png";
                    case ReviewStatus.NotStarted:
                        return "icon_circle.png";
                    default:
                        return "icon_circle.png";
                }
            }

            return "icon_circle.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
