﻿using System;

namespace WordWeaver.Models
{
    public sealed class TranslationHistory : DbObject
    {
        public string SourceLanguage { get; set; }

        public string SourceText { get; set; }

        public string TranslationLanguage { get; set; }

        public string TranslatedText { get; set; }

        public DateTime Date { get; set; }
    }
}
