﻿using WorkTimeLog.Services;

namespace WorkTimeLog
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

            builder.Services.AddSingleton<AppDbContext>();
            builder.Services.AddSingleton<LocalizationService>();

            return builder.Build();
        }
    }
}
