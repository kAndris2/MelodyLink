using Microsoft.Extensions.Options;
using MelodyLink.Interfaces;
using MelodyLink.Models;

namespace MelodyLink.Services.Local
{
    public class LocalMusicService : IFlowService
    {
        private readonly LocalStorageSettings _config;

        public LocalMusicService(IOptions<LocalStorageSettings> config)
        {
            _config = config.Value;
        }

        public async Task<IEnumerable<string>> GetMusicTitles()
        {
            if (!Directory.Exists(_config.FilePath))
            {
                throw new ArgumentException($"The filepath does not exist! ({_config.FilePath})");
            }

            var files = Directory.GetFiles(_config.FilePath);

            if (files == null || files.Length == 0)
            {
                throw new ArgumentException($"There are no files on the configred path! ({_config.FilePath})");
            }

            var relevantFiles = files.Where(file => _config.Extensions.Contains(Path.GetExtension(file)))
                .Select(Path.GetFileNameWithoutExtension);

            if (!relevantFiles.Any())
            {
                throw new ArgumentException($"There were no relevant files on the configred path! ({_config.FilePath})");
            }

            return relevantFiles;
        }
    }
}