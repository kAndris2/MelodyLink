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
            try
            {
                _logger.LogInformation($"The application is starting in mode: {_config.Source} to {_config.Target}");

                var sourceFlowService = GetFlowService(_config.Source);
                var musicTracks = await sourceFlowService.GetMusicTracks();

                _logger.LogInformation($"Found {musicTracks.Count()} track(s) on the {_config.Source}.");

                var targetFlowService = GetFlowService(_config.Target);
                await targetFlowService.SyncMusicTracks(musicTracks);

                _logger.LogInformation($"The music track(s) have been successfully syncronized to {_config.Target}!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"The process has stopped because of an exception! Ex.: {ex.Message}");
            }
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