using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MelodyLink.Enums;
using MelodyLink.Interfaces;
using MelodyLink.Models;
using MelodyLink.Services.Spotify;
using MelodyLink.Services.Local;

namespace MelodyLink
{
    public class Application
    {
        private readonly ILogger<Application> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly SyncSettings _config;

        public Application(IServiceProvider serviceProvider, IOptions<SyncSettings> config, ILogger<Application> logger)
        {
            _serviceProvider = serviceProvider;
            _config = config.Value;
            _logger = logger;
        }

        public async Task Run()
        {
            _logger.LogInformation($"The application is starting in mode: {_config.Source} to {_config.Target}");

            var sourceFlowService = GetFlowService(_config.Source);
            var musicTitles = await sourceFlowService.GetMusicTitles();

            if (musicTitles == null) return;

            _logger.LogInformation($"Found {musicTitles.Count()} track(s) on the source.");
        }

        private IFlowService GetFlowService(Flow flow)
        {
            return flow switch
            {
                Flow.PC => _serviceProvider.GetRequiredService<LocalMusicService>(),
                Flow.Spotify => _serviceProvider.GetRequiredService<SpotifyService>(),
                _ => throw new ArgumentException($"Unknown flow! ({flow})")
            };
        }
    }
}