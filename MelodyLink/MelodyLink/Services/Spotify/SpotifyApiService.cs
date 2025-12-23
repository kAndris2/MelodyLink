using SpotifyAPI.Web;
using MelodyLink.Models;

namespace MelodyLink.Services.Spotify
{
    public class SpotifyApiService
    {
        private readonly SpotifyClient _client;

        public SpotifyApiService(SpotifyConnector connector)
        {
            _client = connector.Connect()
                .GetAwaiter()
                .GetResult();
        }

        public async Task<Dictionary<string, List<SpotifyMusicTrack>>> GetPlaylistTracks()
        {
            var playlistTracks = new Dictionary<string, List<SpotifyMusicTrack>>();
            var playlists = await GetPlaylists();

            foreach (var playlist in playlists)
            {
                var fullTracks = await GetFullTracks(playlist.Id);
                var musicTracks = fullTracks.Select(track => new SpotifyMusicTrack(track.Name, [.. track.Artists.Select(artist => artist.Name)])).ToList();
                playlistTracks.Add(playlist.Name, musicTracks);
            }

            return playlistTracks;
        }

        private async Task<List<FullTrack>> GetFullTracks(string playlistId)
        {
            var tracks = new List<FullTrack>();
            var page = await _client.Playlists.GetItems(playlistId);

            AddTracks(page, tracks);

            while (page.Next != null)
            {
                page = await _client.NextPage(page);
                AddTracks(page, tracks);
            }

            return tracks;
        }

        private static void AddTracks(Paging<PlaylistTrack<IPlayableItem>> page, List<FullTrack> tracks)
        {
            foreach (var item in page.Items)
            {
                if (item.Track is FullTrack track)
                {
                    tracks.Add(track);
                }
            }
        }

        private async Task<List<FullPlaylist>> GetPlaylists()
        {
            var playlist = new List<FullPlaylist>();
            var page = await _client.Playlists.CurrentUsers();

            do
            {
                try
                {
                    playlist.AddRange(page.Items);
                    page = await _client.NextPage(page);
                }
                catch
                {
                    break;
                }
            }
            while (page.Next != null);

            return playlist;
        }
    }
}