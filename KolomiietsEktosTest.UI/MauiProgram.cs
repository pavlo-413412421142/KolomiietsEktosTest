using KolomiietsEktosTest.UI.Services;
using KolomiietsEktosTest.UI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace KolomiietsEktosTest.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Load appsettings.json from app package
            using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").Result;
            builder.Configuration.AddJsonStream(stream);

            // Select platform-specific key from single config file
            string platformKey;

            if (DeviceInfo.Platform == DevicePlatform.WinUI)
                platformKey = "Windows";
            else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                platformKey = "Other";
            else
                throw new NotSupportedException("Unsupported platform");

            // Get connection string & DB name
            string mongoConn = builder.Configuration[$"MongoDb:{platformKey}:ConnectionString"];
            string dbName = builder.Configuration["DbName"] ?? "BinaryDB";

            var mongoUrl = new MongoUrl(mongoConn);

            // Register MongoDB
            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var client = new MongoClient(mongoUrl);
                return client.GetDatabase(dbName);
            });

            builder.Services.AddSingleton<DataService>();
            builder.Services.AddTransient<ItemsViewModel>();
            builder.Services.AddTransient<Views.ItemsView>();

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

            return builder.Build();
        }
    }
}
