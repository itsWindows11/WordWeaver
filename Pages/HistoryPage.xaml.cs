using CommunityToolkit.Mvvm.DependencyInjection;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WordWeaver.Models;
using WordWeaver.ViewModels;

namespace WordWeaver.Pages;

public sealed partial class HistoryPage : Page
{
    public HistoryPageViewModel ViewModel { get; } = Ioc.Default.GetRequiredService<HistoryPageViewModel>();

    public HistoryPage()
    {
        InitializeComponent();

        ViewModel.GetTranslationHistoryCommand?.Execute(null);

        OnTranslationHistoryCollectionChanged(null, null);
        ViewModel.TranslationHistory.CollectionChanged += OnTranslationHistoryCollectionChanged;
    }

    private void OnTranslationHistoryCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (!ViewModel.TranslationHistory.Any())
            VisualStateManager.GoToState(this, "NoHistoryState", false);
        else
            VisualStateManager.GoToState(this, "HistoryAvailableState", false);
    }

    private void OnPageUnloaded(object sender, RoutedEventArgs e)
    {
        ViewModel.TranslationHistory.CollectionChanged -= OnTranslationHistoryCollectionChanged;
    }

    private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        var item = (TranslationHistory)((FrameworkElement)e.OriginalSource).DataContext;

        ViewModel.RemoveHistoryItemCommand?.Execute(item);
    }
}
