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
                    case ReviewStatus.Approved:
                        return "icon_success.png";
                    case ReviewStatus.RejectedByDriver:
                    case ReviewStatus.RejectedByReviewer:
                        return "icon_error.png";
                    case ReviewStatus.NotStarted:
                        return "icon_circle.png";
                    default:
                        return "circle_outlined.png";
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
