using System.Text.Json.Serialization;

#nullable enable

namespace WordWeaver.Models
{
    public sealed class LibreTranslationInfo
    {
        [JsonPropertyName("q")]
        public string? Query { get; set; }

        [JsonPropertyName("source")]
        public string? Source { get; set; }

        [JsonPropertyName("target")]
        public string? Target { get; set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }

        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; } = string.Empty;

        [JsonPropertyName("secret")]
        public string Secret { get; set; } = string.Empty;
    }
}
