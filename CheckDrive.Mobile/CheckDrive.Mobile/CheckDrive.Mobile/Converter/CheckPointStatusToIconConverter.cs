using CheckDrive.Mobile.Models.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Converter
{
    public class CheckPointStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CheckPointStatus status)
            {
                if (status == CheckPointStatus.Completed)
                {
                    return "icon_success.png";
                }

                return status == CheckPointStatus.Interrupted ? "icon_warning.png" : "icon_error.png";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
