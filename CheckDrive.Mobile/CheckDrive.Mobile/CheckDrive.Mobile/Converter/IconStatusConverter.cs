using System;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Converter
{
    public class IconStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((Enum)value)
            {
                //case StatusForDto.Completed: return "icon_check.png";
                //case StatusForDto.Pending: return "icon_circle.png";
                //case StatusForDto.Rejected: return "icon_faild.png";
                //case StatusForDto.Unassigned: return "icon_circle.png";
                default: return "icon_circle.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
