using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using WordWeaver.Models;
using WordWeaver.Services;

namespace WordWeaver.ViewModels
{
    public sealed partial class MainPageViewModel : ObservableObject
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
        private LanguageInfo _selectedSourceLangInfo;

        [ObservableProperty]
        private LanguageInfo _selectedTranslationLangInfo;

        [RelayCommand]
        private async Task TranslateAsync()
        {
            if (string.IsNullOrWhiteSpace(SourceText)) return;

            TranslatedText = await Ioc.Default
                .GetRequiredService<ITranslationService>()
                .TranslateAsync(SourceText, SelectedSourceLangInfo.LanguageCode, SelectedTranslationLangInfo.LanguageCode);
        }
    }
}
