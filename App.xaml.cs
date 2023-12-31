﻿using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TenMica;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WordWeaver.Enums;
using WordWeaver.Services;
using WordWeaver.ViewModels;
using WordWeaver.Constants;

namespace WordWeaver;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public sealed partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
        Suspending += OnSuspending;

#if !DEBUG
        AppCenter.Start(ApiConstants.AppCenterSecret, typeof(Analytics), typeof(Crashes));

        UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
#endif

        ConfigureServices();
    }

    public void ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        // Services
        if (ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(SettingsService.SelectedService), out object value)
            && value is int @enum
            && @enum == (int)SupportedTranslationServices.GoogleTranslate)
        {
            serviceCollection.AddSingleton<ITranslationService, GoogleTranslateService>();
        }
        else
        {
            serviceCollection.AddSingleton<ITranslationService, LibreTranslateService>();
        }

        serviceCollection.AddSingleton<IRepositoryService, RepositoryService>();
        serviceCollection.AddSingleton<SettingsService>();

        // View Models
        serviceCollection.AddSingleton<HomePageViewModel>();
        serviceCollection.AddSingleton<HistoryPageViewModel>();

        Ioc.Default.ConfigureServices(serviceCollection.BuildServiceProvider());
    }

    private void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        Crashes.TrackError(e.Exception, new Dictionary<string, string>()
        {
            { "source", "XamlUnhandledExceptionEvent" }
        });
    }

    private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        Crashes.TrackError(e.Exception, new Dictionary<string, string>()
        {
            { "source", "TaskSchedulerUnobservedTaskException" }
        });
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (Window.Current.Content is not Frame rootFrame)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();

            rootFrame.NavigationFailed += OnNavigationFailed;

            // Place the frame in the current Window
            Window.Current.Content = rootFrame;

            rootFrame.RequestedTheme = Ioc.Default.GetRequiredService<SettingsService>().Theme;
            rootFrame.Background = new TenMicaBrush();
        }

        if (!e.PrelaunchActivated)
        {
            CoreApplication.EnablePrelaunch(true);

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(LoadingPage), e.Arguments);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    /// <summary>
    /// Invoked when application execution is being suspended.  Application state is saved
    /// without knowing whether the application will be terminated or resumed with the contents
    /// of memory still intact.
    /// </summary>
    /// <param name="sender">The source of the suspend request.</param>
    /// <param name="e">Details about the suspend request.</param>
    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
        var deferral = e.SuspendingOperation.GetDeferral();
        //TODO: Save application state and stop any background activity
        deferral.Complete();
    }
}
