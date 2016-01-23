using System;
using System.Globalization;
using System.Windows.Data;

namespace Duelyst.DeckConstructor.ParalaxBinary
{
    public class ParallaxConverter : IValueConverter
    {
        const double _factor = -0.10;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double factor;
            if (!Double.TryParse(parameter as string, out factor))
            {
                factor = _factor;
            }

            if (value is double)
            {
                return (double)value * factor;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double factor;
            if (!Double.TryParse(parameter as string, out factor))
            {
                factor = _factor;
            }

            if (value is double)
            {
                return (double)value / factor;
            }
            return 0;
        }
    }
}
