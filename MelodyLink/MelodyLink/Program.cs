using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MelodyLink;
using MelodyLink.Models;
using MelodyLink.Services.Spotify;
using MelodyLink.Services.Local;

var host = Host.CreateDefaultBuilder(args)
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

static void Configure<T>(IServiceCollection services, HostBuilderContext context) 
    where T : class, new()
{
    services.Configure<T>(
        context.Configuration.GetSection(typeof(T).Name)
    );
}