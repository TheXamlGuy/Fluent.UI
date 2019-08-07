using System;
using System.Globalization;
using System.Windows.Data;

namespace Fluent.UI.Controls.Converters
{
    internal class TimeSpanToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = new DateTime();
            if (value != null)
                return dateTime + (TimeSpan) value;
            return dateTime;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
