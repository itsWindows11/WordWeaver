using System.Text.Json.Serialization;

namespace WordWeaver.Models;

public sealed class LibreSupportedLanguageItem
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
