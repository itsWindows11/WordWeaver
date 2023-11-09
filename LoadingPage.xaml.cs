using CommunityToolkit.Mvvm.DependencyInjection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WordWeaver.Services;

namespace WordWeaver;

public sealed partial class LoadingPage : Page
{
    private ITranslationService service = Ioc.Default.GetRequiredService<ITranslationService>();

    public LoadingPage()
    {
        InitializeComponent();
    }

    private async void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        await service.FetchSupportedLanguagesAsync();
        Frame.Navigate(typeof(MainPage));
    }
}
