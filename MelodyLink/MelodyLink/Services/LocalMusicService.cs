using Microsoft.Extensions.Options;
using MelodyLink.Models;
using MelodyLink.Interfaces;

namespace MelodyLink.Services
{
    public class LocalMusicService : IFlowService
    {
        private readonly LocalStorageSettings _config;

        public LocalMusicService(IOptions<LocalStorageSettings> config)
        {
            _config = config.Value;
        }

        async Task<IEnumerable<string>> IFlowService.GetMusicTitles()
        {
            if (!Directory.Exists(_config.FilePath))
            {
                throw new ArgumentException($"The filepath does not exist! ({_config.FilePath})");
            }

            return Directory.GetFiles(_config.FilePath)
                .Where(file => _config.Extensions.Contains(Path.GetExtension(file)))
                .Select(Path.GetFileNameWithoutExtension);
        }
    }
}