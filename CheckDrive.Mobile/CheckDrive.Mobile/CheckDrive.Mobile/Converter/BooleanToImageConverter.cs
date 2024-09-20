using System;
using System.Globalization;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Converter
{
    public class BooleanToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "IsAllTrue")
            {
                if (value is bool isAllTrue)
                {
                    return isAllTrue ? "icon_check.png" : "icon_incorrect.png";
                }
            }
            else if (value is bool booleanValue)
            {
                return booleanValue ? "icon_correct_for_history.png" : "icon_incorrect_for_history.png";
            }
            return "icon_incorrect.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
