using System.Globalization;

namespace KolomiietsEktosTest.UI.Converters
{
    public class UnixToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is uint unix)
            {
                var date = DateTimeOffset.FromUnixTimeSeconds(unix).LocalDateTime;
                return date.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
