using SpotifyAPI.Web;

namespace MelodyLink.Services.Spotify
{
    public class SpotifyApiService
    {
        private readonly SpotifyClient _client;
        private readonly SyncronousCommandRunner _runner;

        public SpotifyApiService(SpotifyConnector connector, SyncronousCommandRunner runner)
        {
            _runner = runner;
            _client = runner.Run(() => connector.Connect());
        }

        public PrivateUser GetUserProfile()
        {
            return _runner.Run(() => _client.UserProfile.Current());
        }
    }
}