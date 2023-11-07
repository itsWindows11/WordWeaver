using System;

namespace WordWeaver.Models;

public sealed class NavigationItem
{
    public string Label { get; set; }

    public string IconGlyph { get; set; }

    public Type PageType { get; set; }

    public NavigationItem(string label, string iconGlyph, Type pageType)
    {
        Label = label;
        IconGlyph = iconGlyph;
        PageType = pageType;
    }
}
