using MelodyLink.Interfaces;

namespace MelodyLink.Services.Spotify
{
    public class SpotifyService : IFlowService
    {
        private readonly SpotifyApiService _spotify;

        public SpotifyService(SpotifyApiService spotify)
        {
            _spotify = spotify;
        }

        public async Task<IEnumerable<string>> GetMusicTitles()
        {
            return _spotify.GetPlaylistTracks();
        }
    }
}