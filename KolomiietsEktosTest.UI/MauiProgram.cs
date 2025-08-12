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

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var mongoConn = builder.Configuration["MongoDb:ConnectionString"];

            // Parse it to get DB name automatically
            var mongoUrl = new MongoUrl(mongoConn);

            // Register MongoDB
            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var client = new MongoClient(mongoUrl);
                return client.GetDatabase(mongoUrl.DatabaseName);
            });

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
