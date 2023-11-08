using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WordWeaver.Models;
using WordWeaver.Services;

namespace WordWeaver.ViewModels;

public sealed partial class HistoryPageViewModel : ObservableObject
{
    public ObservableCollection<TranslationHistory> TranslationHistory { get; } = new();

    [RelayCommand]
    public async Task GetTranslationHistoryAsync()
    {
        foreach (var item in await Ioc.Default
            .GetRequiredService<IRepositoryService>()
            .GetSavedTranslationsAsync())
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
}
