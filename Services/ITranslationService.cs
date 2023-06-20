using System.Collections.Generic;
using System.Threading.Tasks;
using WordWeaver.Models;

namespace WordWeaver.Services
{
    public interface ITranslationService
    {
        /// <summary>
        /// Gets a list of source languages supported by this translation service.
        /// </summary>
        IList<LanguageInfo> SupportedSourceLanguages { get; }

        /// <summary>
        /// Gets a list of translation languages supported by this translation service.
        /// </summary>
        IList<LanguageInfo> SupportedTranslationLanguages { get; }

        /// <summary>
        /// Translates text from a certain language to another.
        /// </summary>
        /// <param name="text">The text to translate.</param>
        /// <param name="fromLocale">The source language.</param>
        /// <param name="toLocale">The language that will be translated to.</param>
        /// <returns>The translated string.</returns>
        Task<string> TranslateAsync(string text, string fromLocale, string toLocale);
    }
}
