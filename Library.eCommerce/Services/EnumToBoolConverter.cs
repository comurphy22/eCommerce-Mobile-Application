using System.Globalization;
using Microsoft.Maui.Controls; 

namespace Maui.eCommerce.ViewModels;

public class EnumToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        return value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return null;

        if ((bool)value)
            return parameter;

        return null;
    }
}