using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.UI;
using System;
using System.ComponentModel;
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

    private DispatcherTimer _timer;
    private bool _shouldTrigger;

    private ITranslationService service = Ioc.Default.GetRequiredService<ITranslationService>();
    private SettingsService settingsService = Ioc.Default.GetRequiredService<SettingsService>();

    public HomePage()
    {
        InitializeComponent();

        _timer = new()
        {
            Interval = TimeSpan.FromMilliseconds(2000)
        };

        ViewModel.PropertyChanging += OnViewModelPropertyChanging;

        ViewModel.GetTranslationHistoryCommand?.Execute(null);
    }

    private void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        if (settingsService.IsHistoryEnabled)
        {
            OnTranslationHistoryCollectionChanged(null, null);
            ViewModel.TranslationHistory.CollectionChanged += OnTranslationHistoryCollectionChanged;
        }

        if (!string.IsNullOrEmpty(ViewModel.SourceText))
        {
            SourceTextBox.Text = ViewModel.SourceText;
        }

        _timer.Start();
        _timer.Tick += OnTimerTick;
    }

    private void OnPageUnloaded(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _timer.Tick -= OnTimerTick;

        ViewModel.PropertyChanging -= OnViewModelPropertyChanging;

        if (settingsService.IsHistoryEnabled)
            ViewModel.TranslationHistory.CollectionChanged -= OnTranslationHistoryCollectionChanged;
    }
}

// Events
public partial class HomePage
{
    private void OnTranslationHistoryCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (!ViewModel.TranslationHistory.Any())
            VisualStateManager.GoToState(this, "NoHistoryState", false);
        else
            VisualStateManager.GoToState(this, "HistoryAvailableState", false);
    }

    private void OnViewModelPropertyChanging(object sender, PropertyChangingEventArgs e)
    {
        ViewModel.PropertyChanging -= OnViewModelPropertyChanging;

        if (e.PropertyName == nameof(ViewModel.SelectedSourceLangInfoIndex))
        {
            if (TranslationComboBox.SelectedItem == SourceComboBox.SelectedItem
                && ViewModel.SelectedSourceLanguageInfo.LanguageCode != "auto")
            {
                var previousSourceLangInfo = ViewModel.SelectedSourceLanguageInfo;
                var previousTranslationLangInfo = ViewModel.SelectedTranslationLangInfo;

                ViewModel.SelectedTranslationLangInfo = previousSourceLangInfo;
                ViewModel.SelectedSourceLanguageInfo = previousTranslationLangInfo;
            } else if (TranslationComboBox.SelectedItem == SourceComboBox.SelectedItem)
            {
                ViewModel.SelectedTranslationLangInfoIndex = ViewModel.SelectedSourceLangInfoIndex + 2;
            }
        } else if (e.PropertyName == nameof(ViewModel.SelectedTranslationLangInfoIndex))
        {
            if (TranslationComboBox.SelectedItem == SourceComboBox.SelectedItem
                && ViewModel.SelectedSourceLanguageInfo.LanguageCode != "auto")
            {
                var previousSourceLangInfo = ViewModel.SelectedSourceLanguageInfo;
                var previousTranslationLangInfo = ViewModel.SelectedTranslationLangInfo;

                ViewModel.SelectedTranslationLangInfo = previousSourceLangInfo;
                ViewModel.SelectedSourceLanguageInfo = previousTranslationLangInfo;
            }
        }

        ViewModel.PropertyChanging += OnViewModelPropertyChanging;
    }

    private void OnTimerTick(object sender, object e)
    {
        var oldText = ViewModel.SourceText;
        ViewModel.SourceText = SourceTextBox.Text;

        if (!string.IsNullOrEmpty(ViewModel.SourceText)
            && oldText != ViewModel.SourceText
            && _shouldTrigger)
        {
            _shouldTrigger = false;
            ViewModel.TranslateCommand?.Execute(false);
        }
    }

    private void OnSourceTextBoxTextChanged(object sender, TextChangedEventArgs args)
    {
        _shouldTrigger = true;
    }

    private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        var item = (TranslationHistory)((FrameworkElement)e.OriginalSource).DataContext;

        ViewModel.RemoveHistoryItemCommand?.Execute(item);
    }
}

// Commands
public partial class HomePage
{
    [RelayCommand]
    private void OpenHistoryPage()
    {
        Frame.Navigate(typeof(HistoryPage));
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
}