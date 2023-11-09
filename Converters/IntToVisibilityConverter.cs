using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WordWeaver.Converters
{
    public sealed class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = (int)value;

            if (parameter == null)
                return val > 0 ? Visibility.Visible : Visibility.Collapsed;

            return val >= int.Parse((string)parameter) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
