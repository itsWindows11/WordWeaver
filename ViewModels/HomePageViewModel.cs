using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WordWeaver.Models;
using WordWeaver.Services;

namespace WordWeaver.ViewModels;

public sealed partial class HomePageViewModel : ObservableObject
{
    [ObservableProperty]
    private long _sourceCharCount;

    [ObservableProperty]
    private long _translationCharCount;

    [ObservableProperty]
    private string _sourceText;

    [ObservableProperty]
    private string _translatedText;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedSourceLanguageInfo))]
    private int _selectedSourceLangInfoIndex = 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedTranslationLangInfo))]
    private int _selectedTranslationLangInfoIndex = 1;

    private ITranslationService _translationService;
    private SettingsService _settingsService;

    public LanguageInfo SelectedSourceLanguageInfo
    {
        get => _translationService.SupportedSourceLanguages[SelectedSourceLangInfoIndex];
        set => SelectedSourceLangInfoIndex = _translationService.SupportedSourceLanguages.IndexOf(value);
    }

    public LanguageInfo SelectedTranslationLangInfo
    {
        get => _translationService.SupportedTranslationLanguages[SelectedTranslationLangInfoIndex];
        set => SelectedTranslationLangInfoIndex = _translationService.SupportedTranslationLanguages.IndexOf(value);
    }

    public ObservableCollection<TranslationHistory> TranslationHistory { get; } = new();

    public HomePageViewModel(ITranslationService service, SettingsService settingsService)
    {
        _translationService = service;
        _settingsService = settingsService;
    }

    [RelayCommand]
    public async Task GetTranslationHistoryAsync()
    {
        TranslationHistory.Clear();

        foreach (var item in await Ioc.Default
            .GetRequiredService<IRepositoryService>()
            .GetSavedTranslationsAsync(5))
        {
            TranslationHistory.Add(item);
        }
    }

    [RelayCommand]
    public async Task ClearTranslationHistoryAsync()
    {
        await Ioc.Default.GetRequiredService<IRepositoryService>().ClearHistoryAsync();
        TranslationHistory.Clear();
    }

    [RelayCommand]
    public async Task RemoveHistoryItemAsync(TranslationHistory history)
    {
        await Ioc.Default.GetRequiredService<IRepositoryService>().DeleteHistoryItemAsync(history);
        TranslationHistory.Remove(history);
    }

    [RelayCommand]
    private async Task TranslateAsync(bool shouldSaveToHistory)
    {
        if (string.IsNullOrWhiteSpace(SourceText)) return;

        SourceCharCount = SourceText.Length;

        TranslatedText = await Ioc.Default
            .GetRequiredService<ITranslationService>()
            .TranslateAsync(SourceText,
                SelectedSourceLanguageInfo.LanguageCode,
                SelectedTranslationLangInfo.LanguageCode);

        TranslationCharCount = TranslatedText.Length;

        if (!shouldSaveToHistory || !_settingsService.IsHistoryEnabled)
            return;

        var item = new TranslationHistory()
        {
            SourceText = SourceText,
            TranslatedText = TranslatedText,
            SourceLanguage = SelectedSourceLanguageInfo.LanguageCode,
            TranslationLanguage = SelectedTranslationLangInfo.LanguageCode,
            Date = DateTime.UtcNow
        };

        if (TranslationHistory.Count < 5)
            TranslationHistory.Add(item);

        await Ioc.Default.GetRequiredService<IRepositoryService>().AddSavedTranslationAsync(item);
    }
}
