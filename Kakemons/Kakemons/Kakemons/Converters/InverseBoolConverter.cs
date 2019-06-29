using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Kakemons.UI.Converters
{
    [Preserve(AllMembers = true)]
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool) value);
        }
    }
}
