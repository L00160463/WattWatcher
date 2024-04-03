﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WattWatcher.Data;
using BlazorBootstrap;
using System.Net.Http;

namespace WattWatcher
{
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<ElectricService>();
            builder.Services.AddScoped<AverageService>();
            builder.Services.AddScoped<HttpClient>();

            builder.Services.AddBlazorBootstrap();

            return builder.Build();
        }
    }
}
