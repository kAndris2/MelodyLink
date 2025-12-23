using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web;
using MelodyLink.Models;

namespace MelodyLink.Services.Spotify
{
    public class SpotifyConnector
    {
        private readonly ILogger<SpotifyConnector> _logger;
        private readonly TaskCompletionSource<SpotifyClient> _clientSource = new();
        private readonly SpotifySettings _config;
        private EmbedIOAuthServer _server;

        public SpotifyConnector(IOptions<SpotifySettings> config, ILogger<SpotifyConnector> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public async Task<SpotifyClient> Connect()
        {
            _logger.LogInformation("The application is about to connect to Spotify...");

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

            _logger.LogInformation("The authorization was successful!");

            var spotify = new SpotifyClient(tokenResponse.AccessToken);
            _clientSource.TrySetResult(spotify);
        }

        private async Task OnErrorReceived(object sender, string error, string state)
        {
            _logger.LogError($"Aborting authorization! Ex.: {error}");
            await _server.Stop();
        }
    }
}