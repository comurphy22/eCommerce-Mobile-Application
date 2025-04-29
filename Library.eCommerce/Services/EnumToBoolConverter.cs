using System.Globalization;
using Microsoft.Maui.Controls;

namespace Maui.eCommerce.Converters;

public class EnumToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null) return false;
        return value.ToString() == parameter.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null) return null;
        if ((bool)value)
            return Enum.Parse(targetType, parameter.ToString());
        return null;
    }
}