using System;
using System.Globalization;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Converter
{
    public class BooleanToReviewStatusIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isApproved)
            {
                return isApproved ? "icon_success.png" : "icon_error.png";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
