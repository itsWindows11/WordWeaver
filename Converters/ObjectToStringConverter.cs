using System;
using Windows.UI.Xaml.Data;
using WordWeaver.Enums;

namespace WordWeaver.Converters;

public sealed class ObjectToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is SupportedTranslationServices @enum)
        {
            return @enum switch
            {
                SupportedTranslationServices.LibreTranslate => "LibreTranslate",
                SupportedTranslationServices.GoogleTranslate => "Google Translate",
                _ => @enum.ToString()
            };
        }

        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}