using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Windows.Web.Http;
using WordWeaver.Models;

namespace WordWeaver.Services;

public sealed partial class GoogleTranslateService : ITranslationService
{
    private HttpClient _client = new();

    public IList<LanguageInfo> SupportedSourceLanguages { get; private set; }

    public IList<LanguageInfo> SupportedTranslationLanguages { get; private set; }

    public async Task FetchSupportedLanguagesAsync()
    {
        // For the sake of testing the network connection before using the API,
        // we'll just get translate.google.com and only ensure that the request
        // is successful.

        using var response = await _client.GetAsync(new Uri("https://translate.google.com"));
        response.EnsureSuccessStatusCode();

        SupportedSourceLanguages = SupportedTranslationLanguages = languageDictionary.Select(x => new LanguageInfo(x.Key, x.Value)).ToList();
        SupportedTranslationLanguages.Insert(0, new("Auto", "auto"));
    }

    public async Task<string> TranslateAsync(string text, string fromLocale, string toLocale)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("https://translate.googleapis.com/translate_a/single?client=gtx&sl=");
        stringBuilder.Append(fromLocale);
        stringBuilder.Append("&tl=");
        stringBuilder.Append(toLocale);
        stringBuilder.Append("&dt=t&q=");
        stringBuilder.Append(HttpUtility.UrlEncode(text));

        var response = await (await _client.GetAsync(new Uri(stringBuilder.ToString()))).Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(response) || response.StartsWith("<html"))
            return string.Empty;

        using JsonDocument document = JsonDocument.Parse(response);

        JsonElement root = document.RootElement;
        JsonElement translations = root[0];

        var combinedStringBuilder = new StringBuilder();
        var whitespace = " ";

        foreach (JsonElement translation in translations.EnumerateArray())
        {
            string sentence = translation[0].GetString();
            combinedStringBuilder.Append(sentence + whitespace);
        }

        return combinedStringBuilder.ToString().Trim();
    }
}
public partial class GoogleTranslateService
{
    public readonly Dictionary<string, string> languageDictionary = new()
    {
        { "Afrikaans", "af"},
        { "Albanian", "sq"},
        { "Amharic", "am"},
        { "Arabic", "ar"},
        { "Armenian", "hy"},
        { "Assamese", "as"},
        { "Aymara", "ay"},
        { "Azerbaijani", "az"},
        { "Bambara", "bm"},
        { "Basque", "eu"},
        { "Belarusian", "be"},
        { "Bengali", "bn"},
        { "Bhojpuri", "bho"},
        { "Bosnian", "bs"},
        { "Bulgarian", "bg"},
        { "Catalan", "ca"},
        { "Cebuano", "ceb"},
        { "Chinese (Simplified)", "zh"},
        { "Chinese (Traditional)", "zh-TW"},
        { "Corsican", "co"},
        { "Croatian", "hr"},
        { "Czech", "cs"},
        { "Danish", "da"},
        { "Dhivehi", "dv"},
        { "Dogri", "doi"},
        { "Dutch", "nl"},
        { "English", "en"},
        { "Esperanto", "eo"},
        { "Estonian", "et"},
        { "Ewe", "ee"},
        { "Filipino (Tagalog)", "fil"},
        { "Finnish", "fi"},
        { "French", "fr"},
        { "Frisian", "fy"},
        { "Galician", "gl"},
        { "Georgian", "ka"},
        { "German", "de"},
        { "Greek", "el"},
        { "Guarani", "gn"},
        { "Gujarati", "gu"},
        { "Haitian Creole", "ht"},
        { "Hausa", "ha"},
        { "Hawaiian", "haw"},
        { "Hebrew", "he"},
        { "Hindi", "hi"},
        { "Hmong", "hmn"},
        { "Hungarian", "hu"},
        { "Icelandic", "is"},
        { "Igbo", "ig"},
        { "Ilocano", "ilo"},
        { "Indonesian", "id"},
        { "Irish", "ga"},
        { "Italian", "it"},
        { "Japanese", "ja"},
        { "Javanese", "jw"},
        { "Kannada", "kn"},
        { "Kazakh", "kk"},
        { "Khmer", "km"},
        { "Kinyarwanda", "rw"},
        { "Konkani", "gom"},
        { "Korean", "ko"},
        { "Krio", "kri"},
        { "Kurdish", "ku"},
        { "Kurdish (Sorani)", "ckb"},
        { "Kyrgyz", "ky"},
        { "Lao", "lo"},
        { "Latin", "la"},
        { "Latvian", "lv"},
        { "Lingala", "ln"},
        { "Lithuanian", "lt"},
        { "Luganda", "lg"},
        { "Luxembourgish", "lb"},
        { "Macedonian", "mk"},
        { "Maithili", "mai"},
        { "Malagasy", "mg"},
        { "Malay", "ms"},
        { "Malayalam", "ml"},
        { "Maltese", "mt"},
        { "Maori", "mi"},
        { "Marathi", "mr"},
        { "Meiteilon", "mni-Mtei"},
        { "Mizo", "lus"},
        { "Mongolian", "mn"},
        { "Myanmar", "my"},
        { "Nepali", "ne"},
        { "Norwegian", "no"},
        { "Nyanja", "ny"},
        { "Odia", "or"},
        { "Oromo", "om"},
        { "Pashto", "ps"},
        { "Persian", "fa"},
        { "Polish", "pl"},
        { "Portuguese", "pt"},
        { "Punjabi", "pa"},
        { "Quechua", "qu"},
        { "Romanian", "ro"},
        { "Russian", "ru"},
        { "Samoan", "sm"},
        { "Sanskrit", "sa"},
        { "Scots Gaelic", "gd"},
        { "Sepedi", "nso"},
        { "Serbian", "sr"},
        { "Sesotho", "st"},
        { "Shona", "sn"},
        { "Sindhi", "sd"},
        { "Sinhala (Sinhalese)", "si"},
        { "Slovak", "sk"},
        { "Slovenian", "sl"},
        { "Somali", "so"},
        { "Spanish", "es"},
        { "Sundanese", "su"},
        { "Swahili", "sw"},
        { "Swedish", "sv"},
        { "Tagalog (Filipino)", "tl"},
        { "Tajik", "tg"},
        { "Tamil", "ta"},
        { "Tatar", "tt"},
        { "Telugu", "te"},
        { "Thai", "th"},
        { "Tigrinya", "ti"},
        { "Tsonga", "ts"},
        { "Turkish", "tr"},
        { "Turkmen", "tk"},
        { "Twi (Akan)", "ak"},
        { "Ukrainian", "uk"},
        { "Urdu", "ur"},
        { "Uyghur", "ug"},
        { "Uzbek", "uz"},
        { "Vietnamese", "vi"},
        { "Welsh", "cy"},
        { "Xhosa", "xh"},
        { "Yiddish", "yi"},
        { "Yoruba", "yo"},
        { "Zulu", "zu"},
    };
}