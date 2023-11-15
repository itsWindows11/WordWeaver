﻿using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using WordWeaver.Services;

namespace WordWeaver.Pages;

public sealed partial class SettingsPage : Page
{
    private SettingsService _settingsService = Ioc.Default.GetRequiredService<SettingsService>();

    public SettingsPage()
    {
        InitializeComponent();
    }

    [RelayCommand]
    private Task ClearHistoryAsync()
        => Ioc.Default.GetRequiredService<IRepositoryService>().ClearHistoryAsync();
}
