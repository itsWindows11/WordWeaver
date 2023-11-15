using Windows.UI.Xaml;

namespace WordWeaver.Helpers;

public static class BindingHelpers
{
    public static bool InvertBoolean(bool value)
        => !value;

    public static Visibility InvertBooleanToVisibility(bool value)
        => !value ? Visibility.Visible : Visibility.Collapsed;
}
