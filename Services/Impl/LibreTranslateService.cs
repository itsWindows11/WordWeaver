using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using WordWeaver.Models;

namespace WordWeaver.Services
{
    public sealed class LibreTranslateService : ITranslationService, IDisposable
    {
        private HttpClient _httpClient;

        private IList<LanguageInfo> _supportedSourceLanguages;
        private IList<LanguageInfo> _supportedTranslationLanguages;

        public IList<LanguageInfo> SupportedSourceLanguages
        {
            get
            {
                return _supportedSourceLanguages ??= new List<LanguageInfo>()
                {
                    new("Auto", "auto"),
                    new("English", "en"),
                    new("Arabic", "ar"),
                    new("Azerbaijani", "az"),
                    new("Chinese", "zh"),
                    new("Czech", "cs"),
                    new("Danish", "da"),
                    new("Dutch", "nl"),
                    new("Esperanto", "eo"),
                    new("Finnish", "fi"),
                    new("French", "fr"),
                    new("German", "de"),
                    new("Greek", "el"),
                    new("Hebrew", "he"),
                    new("Hindi", "hi"),
                    new("Hungarian", "hu"),
                    new("Indonesian", "id"),
                    new("Irish", "ga"),
                    new("Italian", "it"),
                    new("Japanese", "ja"),
                    new("Korean", "ko"),
                    new("Persian", "fa"),
                    new("Polish", "pl"),
                    new("Portuguese", "pt"),
                    new("Russian", "ru"),
                    new("Slovak", "sk"),
                    new("Spanish", "es"),
                    new("Swedish", "sv"),
                    new("Turkish", "tr"),
                    new("Ukranian", "uk")
                };
            }
        }

        public IList<LanguageInfo> SupportedTranslationLanguages
        {
            get
            {
                if (_supportedTranslationLanguages == null)
                {
                    var clonedList = SupportedSourceLanguages.ToList();
                    clonedList.RemoveAt(0);

                    _supportedTranslationLanguages = clonedList;
                }

                return _supportedTranslationLanguages;
            }
        }

        public async Task<string> TranslateAsync(string text, string fromLocale, string toLocale)
        {
            _httpClient ??= new HttpClient();

            var info = new LibreTranslationInfo()
            {
                Query = text,
                Format = "text",
                Source = fromLocale,
                Target = toLocale
            };

            var httpContent = new HttpStringContent(JsonSerializer.Serialize(info), UnicodeEncoding.Utf8);
            httpContent.Headers.ContentType = new HttpMediaTypeHeaderValue("application/json");

            HttpRequestMessage message = new()
            {
                Content = httpContent,
                RequestUri = new("https://libretranslate.org/translate"),
                Method = HttpMethod.Post
            };

            using var result = await _httpClient.TrySendRequestAsync(message, HttpCompletionOption.ResponseHeadersRead);

            using var winrtStream = await result.ResponseMessage.Content.ReadAsInputStreamAsync();

            var translationResponse = await JsonSerializer.DeserializeAsync(winrtStream.AsStreamForRead(), JsonSerializationContext.Default.LibreTranslationResponse);

            return translationResponse.TranslatedText;
        }

        public void Dispose()
            => _httpClient.Dispose();
    }
}
