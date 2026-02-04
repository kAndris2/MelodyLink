using Microsoft.Extensions.Logging;
using MelodyLink.Interfaces;
using MelodyLink.Models;

namespace MelodyLink.Services.Spotify
{
    public class SpotifyService : IFlowService
    {
        private readonly SpotifyApiService _spotify;
        private readonly ILogger<SpotifyService> _logger;

        public SpotifyService(SpotifyApiService spotify, ILogger<SpotifyService> logger)
        {
            _spotify = spotify;
            _logger = logger;
        }

        public async Task<IEnumerable<MusicTrack>> GetMusicTracks()
        {
            try
            {
                var playlistTracks = await _spotify.GetPlaylistTracks();

                if (playlistTracks == null || playlistTracks.Count == 0)
                {
                    throw new ArgumentException("There are no playlists in the account you signed in!");
                }

                return [.. playlistTracks.Values
                    .SelectMany(tracks => tracks)
                    .Select(track => new MusicTrack(track.Title, track.Artists))];
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while trying to get playlists/tracks from Spotify! Ex.: {ex.Message}");
            }
        }

        public Task SyncMusicTracks(IEnumerable<MusicTrack> musicTracks)
        {
            throw new NotImplementedException();
        }
    }
}