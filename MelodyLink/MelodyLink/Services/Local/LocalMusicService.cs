using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MelodyLink.Interfaces;
using MelodyLink.Models;

namespace MelodyLink.Services.Local
{
    public class LocalMusicService : IFlowService
    {
        private readonly LocalStorageSettings _config;
        private readonly ILogger<LocalMusicService> _logger;

        public LocalMusicService(IOptions<LocalStorageSettings> config, ILogger<LocalMusicService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public async Task<IEnumerable<string>?> GetMusicTitles()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while trying to get local files! Ex.: {ex.Message}");
                return null;
            }
        }
    }
}