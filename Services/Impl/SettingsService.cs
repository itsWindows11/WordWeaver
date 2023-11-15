using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using WordWeaver.Enums;

namespace WordWeaver.Services;

public sealed partial class SettingsService : ObservableObject
{
    private readonly IPropertySet _localSettings = ApplicationData.Current.LocalSettings.Values;

    public ElementTheme Theme
    {
        get => (ElementTheme)Get((int)ElementTheme.Default);
        set => Set((int)value);
    }

    public SupportedTranslationServices SelectedService
    {
        get => (SupportedTranslationServices)Get((int)SupportedTranslationServices.LibreTranslate);
        set => Set((int)value);
    }

    public bool IsHistoryEnabled
    {
        get => Get(true);
        set => Set(value);
    }
}

public partial class SettingsService
{
    public T Get<T>(T defaultValue = default, [CallerMemberName] string propertyName = null)
    {
        _localSettings[propertyName] ??= defaultValue;
        return (T)_localSettings[propertyName];
    }

    public void Set<T>(T newValue, [CallerMemberName] string propertyName = null)
    {
        var value = (T)_localSettings[propertyName];

        if (EqualityComparer<T>.Default.Equals(value, newValue))
            return;

        if (!_localSettings.ContainsKey(propertyName))
        {
            _localSettings.Add(propertyName, value);
            OnPropertyChanged(propertyName);
            return;
        }

        _localSettings[propertyName] = newValue;
        OnPropertyChanged(propertyName);
    }
}