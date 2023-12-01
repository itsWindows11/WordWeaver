using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        CustomTitleBar.SetTitleBarForCurrentView();
    }

    private async void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        await ((RepositoryService)Ioc.Default.GetRequiredService<IRepositoryService>()).InitializeAsync();

        try
        {
            await service.FetchSupportedLanguagesAsync();
        } catch (Exception ex)
        {
            Crashes.TrackError(ex, new Dictionary<string, string>()
            {
                { "fromLoading", "true" }
            });

            var contentDialog = new ContentDialog
            {
                Title = "Error",
                Content = "An error occurred while connecting to the translation service. Please check your internet connection and try again.",
                PrimaryButtonText = "Exit",
                PrimaryButtonCommand = ExitAppCommand,
                SecondaryButtonText = "Retry",
                SecondaryButtonCommand = RetryCommand
            };

            _ = await contentDialog.ShowAsync();

            return;
        }

        Frame.Navigate(typeof(MainPage));
    }

    [RelayCommand]
    private void ExitApp()
    {
        Application.Current.Exit();
    }

    [RelayCommand]
    private void Retry()
    {
        OnPageLoaded(null, null);
    }
}
