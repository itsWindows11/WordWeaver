using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using WordWeaver.Models;
using WordWeaver.Pages;

namespace WordWeaver;

public sealed partial class MainPage : Page
{
    public IEnumerable<NavigationItem> NavigationItems { get; } = new List<NavigationItem>()
    {
        { new NavigationItem("Home", "\uE10F", typeof(HomePage)) },
        { new NavigationItem("History", "\uE81C", typeof(HistoryPage)) },
        { new NavigationItem("Settings", "\uE115", typeof(SettingsPage)) }
    };

    public MainPage()
    {
        InitializeComponent();
        CustomTitleBar.SetTitleBarForCurrentView();
    }
}
