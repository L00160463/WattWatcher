﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WattWatcher.Data;
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
            builder.Services.AddScoped<HistoricalService>();
            builder.Services.AddScoped<HttpClient>();
            builder.Services.AddScoped<UIDataService>();
            builder.Services.AddScoped<AverageService>();

            // Properly initialize SmsService with configuration
            builder.Services.AddSingleton<SmsService>(sp =>
                new SmsService(
                    "AC2f3034dfe8be1bdf9b31de034b702d47", // Account SID
                    "5176e278c9b14e6c6c56e2d92b770548",                   // Auth Token
                    "+14155238886"                       //WhatsApp-enabled Twilio number
                ));

            builder.Services.AddBlazorBootstrap();

            return builder.Build();
        }
    }
}
