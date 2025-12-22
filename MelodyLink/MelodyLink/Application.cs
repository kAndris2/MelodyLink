using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MelodyLink.Enums;
using MelodyLink.Interfaces;
using MelodyLink.Models;
using MelodyLink.Services.Spotify;
using MelodyLink.Services.Local;

namespace MelodyLink
{
    public class Application
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SyncSettings _config;

        public Application(IServiceProvider serviceProvider, IOptions<SyncSettings> config)
        {
            _serviceProvider = serviceProvider;
            _config = config.Value;
        }

        public async Task Run()
        {
            var sourceFlowService = GetFlowService(_config.Source);
            var musicTitles = await sourceFlowService.GetMusicTitles();
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