using System;
using Windows.Globalization;
using Windows.UI.Xaml.Data;

namespace WordWeaver.Converters
{
    public sealed class LocaleToLanguageLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var str = (string)value;

            if (!string.IsNullOrEmpty(str) && str != "auto")
                return new Language((string)value).DisplayName;
            else if (str == "auto")
                return "Auto";

            throw new ArgumentNullException(nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
