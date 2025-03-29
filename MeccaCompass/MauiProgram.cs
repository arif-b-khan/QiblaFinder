using Microsoft.Extensions.Logging;
using MeccaCompass.Services;
using MeccaCompass.ViewModels;
using MeccaCompass.Views;

namespace MeccaCompass;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register services
        builder.Services.AddSingleton<IGeolocationService, GeolocationService>();
        builder.Services.AddSingleton<ICompassService, CompassService>();
        builder.Services.AddSingleton<IQiblaCalculationService, QiblaCalculationService>();

        // Register pages and view models
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<SettingsViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}