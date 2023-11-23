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

namespace WordWeaver.Services;

public sealed class LibreTranslateService : ITranslationService, IDisposable
{
    private HttpClient _httpClient;

    public IList<LanguageInfo> SupportedSourceLanguages { get; private set; }

    public IList<LanguageInfo> SupportedTranslationLanguages { get; private set; }

    public async Task FetchSupportedLanguagesAsync()
    {
        _httpClient ??= new HttpClient();

        using HttpRequestMessage message = new()
        {
            RequestUri = new("https://libretranslate.org/languages"),
            Method = HttpMethod.Get
        };

        using var result = await _httpClient.TrySendRequestAsync(message, HttpCompletionOption.ResponseHeadersRead);

        using var winrtStream = await result.ResponseMessage.Content.ReadAsInputStreamAsync();

        var translationResponse = await JsonSerializer.DeserializeAsync(winrtStream.AsStreamForRead(), JsonSerializationContext.Default.IListLibreSupportedLanguageItem);

        var items = translationResponse.Select(l => new LanguageInfo(l.Name, l.Code)).ToList();

        SupportedTranslationLanguages = items;

        var sourceItems = items.ToList();
        sourceItems.Insert(0, new LanguageInfo("Auto", "auto"));

        SupportedSourceLanguages = sourceItems;
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

        using var httpContent = new HttpStringContent(JsonSerializer.Serialize(info), UnicodeEncoding.Utf8);
        httpContent.Headers.ContentType = new HttpMediaTypeHeaderValue("application/json");

        using HttpRequestMessage message = new()
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
