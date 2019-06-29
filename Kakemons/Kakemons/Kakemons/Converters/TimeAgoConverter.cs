using System;
using System.Globalization;
using Xamarin.Forms;

namespace Kakemons.UI.Converters
{
    public class TimeAgoConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sent = (DateTimeOffset)value;
            if (sent == null)
                return "";
            var difference = DateTimeOffset.Now.ToUniversalTime() - sent.ToUniversalTime();
            if (difference.TotalDays > 365)
                return $"{Math.Floor(difference.TotalDays / 365)} år siden";
            if (difference.TotalDays > 30)
                return $"{Math.Floor(difference.TotalDays / 30)} måneder siden";
            if (difference.TotalDays > 1)
                return $"{Math.Floor(difference.TotalDays)} dager siden";
            if (difference.TotalHours > 1)
                return $"{Math.Floor(difference.TotalHours)} timer siden";
            if (difference.TotalMinutes > 1)
                return $"{Math.Floor(difference.TotalMinutes)} minutter siden";
            return $"nylig";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
