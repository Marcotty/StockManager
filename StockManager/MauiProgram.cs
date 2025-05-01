using Microsoft.Extensions.Logging;
using StockManager.Pages;
using StockManager.Services;

namespace StockManager
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            
            builder.Services.AddSingleton<IStockService, StockService>();
            builder.Services.AddSingleton<App>();
            builder.Services.AddSingleton<MainPage>();

            // Preload data
            var serviceProvider = builder.Services.BuildServiceProvider();
            var stockService = serviceProvider.GetService<IStockService>();
            stockService?.LoadListFromFile();

            builder.Services.AddTransient<StoragePage>();
            builder.Services.AddTransient<ShoppingPage>();
            builder.Services.AddTransient<RecipesPage>();

            return builder.Build();
        }
    }
}
