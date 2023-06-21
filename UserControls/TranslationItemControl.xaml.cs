using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WordWeaver.Models;

namespace WordWeaver.UserControls
{
    public sealed partial class TranslationItemControl : UserControl
    {
        public TranslationHistory HistoryItem
        {
            get => (TranslationHistory)GetValue(HistoryItemProperty);
            set => SetValue(HistoryItemProperty, value);
        }

        public static DependencyProperty HistoryItemProperty
            = DependencyProperty.Register(nameof(HistoryItem), typeof(TranslationHistory), typeof(TranslationItemControl), new(null));

        public TranslationItemControl()
        {
            InitializeComponent();
        }

        private void Grid_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _ = VisualStateManager.GoToState(this, "HoveredState", false);
        }

        private void Grid_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _ = VisualStateManager.GoToState(this, "NonHoverState", false);
        }
    }
}
