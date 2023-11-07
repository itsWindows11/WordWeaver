using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WordWeaver.Models;
using WordWeaver.Services;
using WordWeaver.ViewModels;

namespace WordWeaver.Pages;

public sealed partial class HomePage : Page
{
    public HomePageViewModel ViewModel { get; } = Ioc.Default.GetRequiredService<HomePageViewModel>();

    private DispatcherQueueTimer _timer;
    private ITranslationService service = Ioc.Default.GetRequiredService<ITranslationService>();

    public HomePage()
    {
        InitializeComponent();

        _timer = DispatcherQueue.GetForCurrentThread().CreateTimer();

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

    [RelayCommand]
    private void Copy(bool isSource = false)
    {
        var dataPackage = new DataPackage()
        {
            RequestedOperation = DataPackageOperation.Copy
        };

        dataPackage.SetText(isSource ? ViewModel.SourceText : ViewModel.TranslatedText);

        Clipboard.SetContent(dataPackage);
    }

    private void OnSourceTextBoxTextChanged(object sender, TextChangedEventArgs args)
    {
        _timer.Debounce(() =>
        {
            var oldText = ViewModel.SourceText;
            ViewModel.SourceText = SourceTextBox.Text;

            if (!string.IsNullOrEmpty(ViewModel.SourceText) && oldText != ViewModel.SourceText)
                ViewModel.TranslateCommand?.Execute(false);
        }, TimeSpan.FromSeconds(1500));
    }

    private void OnSourceComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = (ComboBox)sender;
        ViewModel.SelectedSourceLangInfo = (LanguageInfo)comboBox.SelectedItem;

        ViewModel.TranslateCommand?.Execute(false);
    }

    private void OnTranslationComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = (ComboBox)sender;
        ViewModel.SelectedTranslationLangInfo = (LanguageInfo)comboBox.SelectedItem;
        ViewModel.TranslateCommand?.Execute(false);
    }

    private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        var item = (TranslationHistory)((FrameworkElement)e.OriginalSource).DataContext;
        ViewModel.RemoveHistoryItemCommand?.Execute(item);
    }
}
