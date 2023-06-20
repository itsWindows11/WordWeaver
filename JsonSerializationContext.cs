using System.Text.Json.Serialization;
using WordWeaver.Models;

namespace WordWeaver
{
    [JsonSerializable(typeof(LibreDetectedLanguageInfo))]
    [JsonSerializable(typeof(LibreTranslationResponse))]
    [JsonSerializable(typeof(LibreTranslationInfo))]
    public sealed partial class JsonSerializationContext : JsonSerializerContext
    {
    }
}
