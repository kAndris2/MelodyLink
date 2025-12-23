using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using MelodyLink;
using MelodyLink.Models;
using MelodyLink.Services.Local;
using MelodyLink.Services.Spotify;

try
{
    var host = Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddNLog();
        })
        .ConfigureServices((context, services) =>
        {
            Configure<LocalStorageSettings>(services, context);
            Configure<SyncSettings>(services, context);
            Configure<SpotifySettings>(services, context);

            services.AddTransient<Application>();
            services.AddTransient<LocalMusicService>();
            services.AddTransient<SpotifyService>();
            services.AddTransient<SpotifyConnector>();
            services.AddTransient<SpotifyApiService>();
        })
        .Build();

    var app = host.Services.GetRequiredService<Application>();
    app.Run()
        .GetAwaiter()
        .GetResult();
}
catch (Exception ex)
{
    LogManager.GetCurrentClassLogger()
        .Error($"Unhandled exception! Ex.: {ex.Message}");
}
finally
{
    LogManager.Shutdown();
}

static void Configure<T>(IServiceCollection services, HostBuilderContext context) 
    where T : class, new()
{
    services.Configure<T>(
        context.Configuration.GetSection(typeof(T).Name)
    );
}