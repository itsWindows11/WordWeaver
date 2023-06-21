using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WordWeaver.Models;
using WordWeaver.Services;
using WordWeaver.ViewModels;

namespace WordWeaver
{
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel { get; } = new();

        private DispatcherTimer _timer;
        private bool _shouldTrigger;
        private ITranslationService service = Ioc.Default.GetRequiredService<ITranslationService>();

        public MainPage()
        {
            InitializeComponent();
            CustomTitleBar.SetTitleBarForCurrentView();

            _timer = new()
            {
                Interval = TimeSpan.FromMilliseconds(1500)
            };

            _timer.Tick += OnTimerTick;
            _timer.Start();

            ViewModel.GetTranslationHistoryCommand?.Execute(null);

            OnTranslationHistoryCollectionChanged(null, null);
            ViewModel.TranslationHistory.CollectionChanged += OnTranslationHistoryCollectionChanged;

            Unloaded += OnMainPageUnloaded;
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

        private void OnMainPageUnloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= OnTimerTick;
        }

        private async void OnTimerTick(object sender, object e)
        {
            var oldText = ViewModel.SourceText;
            ViewModel.SourceText = SourceTextBox.Text;

            if (!string.IsNullOrEmpty(ViewModel.SourceText)
                && oldText != ViewModel.SourceText
                && _shouldTrigger)
            {
                _shouldTrigger = false;
                ViewModel.SourceCharCount = ViewModel.SourceText.Length;
                await ViewModel.TranslateCommand?.ExecuteAsync(false);
                ViewModel.TranslationCharCount = ViewModel.TranslatedText.Length;
            }
        }

        private void OnSourceTextBoxTextChanged(object sender, TextChangedEventArgs args)
        {
            _shouldTrigger = true;
        }

        private async void OnSourceComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            ViewModel.SelectedSourceLangInfo = (LanguageInfo)comboBox.SelectedItem;

            await ViewModel.TranslateCommand?.ExecuteAsync(false);
        }

        private async void OnTranslationComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            ViewModel.SelectedTranslationLangInfo = (LanguageInfo)comboBox.SelectedItem;

            await ViewModel.TranslateCommand?.ExecuteAsync(false);
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (TranslationHistory)((FrameworkElement)e.OriginalSource).DataContext;

            ViewModel.RemoveHistoryItemCommand?.Execute(item);
        }
    }
}
