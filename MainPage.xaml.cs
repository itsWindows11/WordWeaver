using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WordWeaver.Models;
using WordWeaver.Pages;

namespace WordWeaver;

public sealed partial class MainPage : Page
{
    public IList<NavigationItem> NavigationItems { get; } = new List<NavigationItem>()
    {
        { new NavigationItem("Home", "\uE10F", typeof(HomePage)) },
        { new NavigationItem("History", "\uE81C", typeof(HistoryPage)) },
        { new NavigationItem("Settings", "\uE115", typeof(SettingsPage)) }
    };

    public static readonly DependencyProperty SelectedNavigationItemIndexProperty =
        DependencyProperty.Register(nameof(SelectedNavigationItemIndex), typeof(int), typeof(MainPage), new PropertyMetadata(0, OnSelectedNavigationItemChanged));

    public int SelectedNavigationItemIndex
    {
        get => (int)GetValue(SelectedNavigationItemIndexProperty);
        set => SetValue(SelectedNavigationItemIndexProperty, value);
    }

    public MainPage()
    {
        InitializeComponent();
        CustomTitleBar.SetTitleBarForCurrentView();

        MainFrame.Navigate(typeof(HomePage));
        MainFrame.Navigated += OnMainFrameNavigated;
    }

    private void OnMainFrameNavigated(object sender, NavigationEventArgs e)
    {
        SelectedNavigationItemIndex = NavigationItems.IndexOf(NavigationItems.FirstOrDefault(n => n.PageType == e.SourcePageType));
    }

    private static void OnSelectedNavigationItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var page = (MainPage)d;
        var mainFrame = page.MainFrame;

        mainFrame.Navigated -= page.OnMainFrameNavigated;

        mainFrame.Navigate(page.NavigationItems[(int)e.NewValue].PageType, null, new SuppressNavigationTransitionInfo());

        mainFrame.Navigated += page.OnMainFrameNavigated;
    }

    private void OnPageUnloaded(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigated -= OnMainFrameNavigated;
    }
}
