using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using MelodyLink;
using MelodyLink.Models;
using MelodyLink.Services.Local;
using MelodyLink.Services.Spotify;
using MelodyLink.Interfaces;
using MelodyLink.Services.Validators;

try
{
    var configValidators = GetConfigValidators();
    var host = Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddNLog();
        })
        .ConfigureServices((context, services) =>
        {
            Configure<LocalStorageSettings>(services, context, configValidators);
            Configure<SyncSettings>(services, context, configValidators);
            Configure<SpotifySettings>(services, context, configValidators);

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

static void Configure<T>(IServiceCollection services, HostBuilderContext context, List<IConfigValidator> configValidators) 
    where T : class, new()
{
    services.Configure<T>(
        GetConfigSection<T>(context, typeof(T).Name, configValidators)
    );
}

static IConfigurationSection GetConfigSection<T>(HostBuilderContext context, string sectionKey, List<IConfigValidator> configValidators)
{
    var section = context.Configuration.GetSection(sectionKey);

    var config = section.Get<T>()
        ?? throw new ArgumentException($"{sectionKey} cannot be null!");

    var configValidator = configValidators.FirstOrDefault(validator => validator.CanValidate(config))
        ?? throw new ArgumentException($"{sectionKey} validator could not be found!");

    configValidator.Validate(config);

    return section;
}

static List<IConfigValidator> GetConfigValidators()
{
    return
    [
        new LocalStorageSettingsValidator(),
        new SpotifySettingsValidator(),
        new SyncSettingsValidator()
    ];
}