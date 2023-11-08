using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WordWeaver.UserControls
{
    public sealed partial class NavigationItemControl : UserControl
    {
        public static readonly DependencyProperty IconGlyphProperty =
            DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(NavigationItemControl), null);

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(NavigationItemControl), null);

        public string IconGlyph
        {
            get => (string)GetValue(IconGlyphProperty);
            set => SetValue(IconGlyphProperty, value);
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public NavigationItemControl()
        {
            InitializeComponent();
        }
    }
}
