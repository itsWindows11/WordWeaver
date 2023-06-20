using System.Text.Json.Serialization;

namespace WordWeaver.Models
{
    public sealed class LibreDetectedLanguageInfo
    {
        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }
    }
}
