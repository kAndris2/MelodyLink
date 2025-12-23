using Microsoft.Extensions.Options;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web;
using MelodyLink.Models;

namespace MelodyLink.Services.Spotify
{
    public class SpotifyConnector
    {
        private readonly TaskCompletionSource<SpotifyClient> _clientSource = new();
        private readonly SpotifySettings _config;
        private EmbedIOAuthServer _server;

        public SpotifyConnector(IOptions<SpotifySettings> config)
        {
            _config = config.Value;
        }

        public async Task<SpotifyClient> Connect()
        {
            var uri = new Uri(_config.RedirectUrl);
            _server = new EmbedIOAuthServer(uri, uri.Port);
            await _server.Start();

            _server.AuthorizationCodeReceived += OnAuthorizationCodeReceived;
            _server.ErrorReceived += OnErrorReceived;

            var request = new LoginRequest(_server.BaseUri, _config.ClientId, LoginRequest.ResponseType.Code)
            {
                Scope =
                [
                    Scopes.PlaylistReadPrivate,
                    Scopes.PlaylistModifyPublic,
                    Scopes.PlaylistReadCollaborative,
                    Scopes.PlaylistModifyPrivate,
                    Scopes.PlaylistModifyPublic,
                    Scopes.UserLibraryModify,
                    Scopes.UserLibraryRead
                ]
            };

            BrowserUtil.Open(request.ToUri());

            return await _clientSource.Task;
        }

        private async Task OnAuthorizationCodeReceived(object sender, AuthorizationCodeResponse response)
        {
            await _server.Stop();

            var config = SpotifyClientConfig.CreateDefault();
            var tokenResponse = await new OAuthClient(config).RequestToken(
              new AuthorizationCodeTokenRequest(
                  _config.ClientId, _config.ClientSecret, response.Code, new Uri(_config.RedirectUrl)
              )
            );

            var spotify = new SpotifyClient(tokenResponse.AccessToken);
            _clientSource.TrySetResult(spotify);
        }

        private async Task OnErrorReceived(object sender, string error, string state)
        {
            Console.WriteLine($"Aborting authorization, error received: {error}");
            await _server.Stop();
        }
    }
}