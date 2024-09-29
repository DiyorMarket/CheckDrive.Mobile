using CheckDrive.Mobile.Models.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Converter
{
    public class ReviewTypeToShowLineBelowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReviewType type)
            {
                switch (type)
                {
                    case ReviewType.DispatcherReview:
                        return false;
                    default:
                        return true;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
