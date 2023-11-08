using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using WordWeaver.Models;

namespace WordWeaver.Services
{
    public sealed class RepositoryService : IRepositoryService
    {
        private SQLiteAsyncConnection _connection;

        public async Task InitializeAsync()
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Database.db", CreationCollisionOption.OpenIfExists);
            _connection = new(file.Path);

            _ = await _connection.CreateTableAsync<TranslationHistory>();
            await _connection.EnableWriteAheadLoggingAsync();
        }

        public Task DeleteHistoryItemAsync(TranslationHistory item)
        {
            return _connection.DeleteAsync(item);
        }

        public Task<TranslationHistory> GetSavedTranslationAsync(Guid id)
        {
            return _connection.GetAsync<TranslationHistory>(id);
        }

        public async Task<IList<TranslationHistory>> GetSavedTranslationsAsync()
        {
            return await _connection.Table<TranslationHistory>().ToListAsync();
        }

        public Task AddSavedTranslationAsync(TranslationHistory item)
        {
            return _connection.InsertOrReplaceAsync(item);
        }

        public Task ClearHistoryAsync()
        {
            return _connection.Table<TranslationHistory>().Where(_ => true).DeleteAsync();
        }

        public async Task<IList<TranslationHistory>> GetSavedTranslationsAsync(int maxCount)
        {
            return await _connection.Table<TranslationHistory>().Take(maxCount).ToListAsync();
        }
    }
}
