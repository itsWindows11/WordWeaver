using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordWeaver.Models;

namespace WordWeaver.Services
{
    public interface IRepositoryService
    {
        Task<IList<TranslationHistory>> GetSavedTranslationsAsync();

        Task<IList<TranslationHistory>> GetSavedTranslationsAsync(int maxCount);

        Task<TranslationHistory> GetSavedTranslationAsync(Guid id);

        Task DeleteHistoryItemAsync(TranslationHistory item);

        Task AddSavedTranslationAsync(TranslationHistory item);

        Task ClearHistoryAsync();
    }
}
