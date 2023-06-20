namespace WordWeaver.Models
{
    public sealed class LanguageInfo
    {
        public string Label { get; set; }

        public string LanguageCode { get; set; }

        public LanguageInfo(string label, string languageCode)
        {
            Label = label;
            LanguageCode = languageCode;
        }

        public override string ToString() => Label;
    }
}
