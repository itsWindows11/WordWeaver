using System.Collections.Generic;
using System.Text.Json.Serialization;
using WordWeaver.Models;

namespace WordWeaver
{
    [JsonSerializable(typeof(LibreDetectedLanguageInfo))]
    [JsonSerializable(typeof(LibreTranslationResponse))]
    [JsonSerializable(typeof(LibreTranslationInfo))]
    [JsonSerializable(typeof(IList<LibreSupportedLanguageItem>))]
    public sealed partial class JsonSerializationContext : JsonSerializerContext
    {
    }
}
