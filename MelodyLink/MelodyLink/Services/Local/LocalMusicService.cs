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
        private readonly MetaDataHandler _metaHandler;

        public LocalMusicService(IOptions<LocalStorageSettings> config, ILogger<LocalMusicService> logger, MetaDataHandler metaHandler)
        {
            _config = config.Value;
            _logger = logger;
            _metaHandler = metaHandler;
        }

        public async Task<IEnumerable<MusicTrack>> GetMusicTracks()
        {
            try
            {
                if (!Directory.Exists(_config.FolderPath))
                {
                    throw new ArgumentException($"The folder path does not exist! ({_config.FolderPath})");
                }

                var filePaths = Directory.GetFiles(_config.FolderPath);

                if (filePaths == null || filePaths.Length == 0)
                {
                    throw new ArgumentException($"There are no files on the configred path! ({_config.FolderPath})");
                }

                var relevantFilePaths = filePaths.Where(file => _config.Extensions.Contains(Path.GetExtension(file)));

                if (!relevantFilePaths.Any())
                {
                    throw new ArgumentException($"There were no relevant files on the configred path! ({_config.FolderPath})");
                }

                return relevantFilePaths.Select(path => _metaHandler.GetMusicTrack(path));
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while trying to get local files! Ex.: {ex.Message}");
            }
        }

        public async Task SyncMusicTracks(IEnumerable<MusicTrack> musicTracks)
        {
            try
            {
                if (!Directory.Exists(_config.FolderPath))
                {
                    throw new ArgumentException($"The folder path does not exist! ({_config.FolderPath})");
                }

                var date = DateTime.Now.ToString("yyyy_MM_dd_-_HH_mm_ss");
                var fileName = $"tracks_({date}).txt";
                var filePath = Path.Combine(_config.FolderPath, fileName);

                using (var writer = new StreamWriter(filePath))
                {
                    foreach (var track in musicTracks)
                    {
                        await writer.WriteLineAsync(track.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during synchronization! Ex.: {ex.Message}");
            }
        }
    }
}