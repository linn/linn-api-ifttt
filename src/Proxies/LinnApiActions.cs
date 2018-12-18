namespace Linn.Api.Ifttt.Proxies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Api.Ifttt.Proxies.Models;
    using Linn.Api.Ifttt.Resources.Linn;
    using Linn.Common.Configuration;
    using Linn.Common.Proxy;

    public class LinnApiActions : ILinnApiActions
    {
        private readonly IRestClient restClient;

        private readonly string apiRoot;

        public LinnApiActions(IRestClient restClient)
        {
            this.restClient = restClient;
            this.apiRoot = ConfigurationManager.Configuration["apiRoot"];
        }

        public async Task<string> TurnOffAllDevices(string accessToken, CancellationToken ct)
        {
            var players = await this.ListAllPlayers(accessToken, ct);

            var tasks = players
                .Select(p => p.Links.FirstOrDefault(l => l.Rel == "standby")?.Href)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(href => new Uri($"{this.apiRoot}{href}"))
                .Select(uri => this.restClient.Put(ct, uri, null, Headers(accessToken), null));

            var statusCodes = await Task.WhenAll(tasks);

            if (statusCodes.Any(c => c != HttpStatusCode.OK))
            {
                throw new LinnApiException(statusCodes.First(c => c != HttpStatusCode.OK));
            }

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<string> TurnOffDevice(string accessToken, string deviceId, CancellationToken ct)
        {
            var uri = new Uri($"{this.apiRoot}/devices/{deviceId}/standby");

            var statusCode = await this.restClient.Put(ct, uri, null, Headers(accessToken), null);

            if (statusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(statusCode);
            }

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<IDictionary<string, string>> GetDeviceNames(string accessToken, CancellationToken ct)
        {
            var players = await this.ListAllPlayers(accessToken, ct);

            return players.ToDictionary(p => p.Id, p => p.Name);
        }

        public async Task<string> PlaySingleMedia(string accessToken, string deviceId, string mediaUri, string mediaTitle, string mediaArtworkUrl, CancellationToken ct)
        {
            var uri = new Uri($"{this.apiRoot}/players/{deviceId}/play");

            var payload = new PlayableItemModel { Uri = mediaUri, Title = mediaTitle, Artwork = mediaArtworkUrl };

            var statusCode = await this.restClient.Put(ct, uri, null, Headers(accessToken), payload);

            if (statusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(statusCode);
            }

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<string> PlayPlaylist(string accessToken, string deviceId, string playlistId, CancellationToken ct)
        {
            var uri = new Uri($"{this.apiRoot}/players/{deviceId}/playlist/");

            var parameters = new Dictionary<string, string> { { "playlistId", playlistId } };

            var statusCode = await this.restClient.Put(ct, uri, parameters, Headers(accessToken), null);

            if (statusCode == HttpStatusCode.Forbidden)
            {
                throw new LinnApiException(HttpStatusCode.BadRequest);
            }

            if (statusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(statusCode);
            }

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<string> MuteDevice(string accessToken, string deviceId, CancellationToken ct)
        {
            var uri = new Uri($"{this.apiRoot}/players/{deviceId}/mute");

            var statusCode = await this.restClient.Put(ct, uri, null, Headers(accessToken), null);

            if (statusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(statusCode);
            }

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<string> UnmuteDevice(string accessToken, string deviceId, CancellationToken ct)
        {
            var uri = new Uri($"{this.apiRoot}/players/{deviceId}/mute");

            var statusCode = await this.restClient.Delete(ct, uri, null, Headers(accessToken));

            if (statusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(statusCode);
            }

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<string> InvokePin(string accessToken, string deviceId, string pinId, CancellationToken ct)
        {
            var uri = new Uri($"{this.apiRoot}/players/{deviceId}/play");

            var parameters = new Dictionary<string, string> { { "pinId", pinId } };

            var statusCode = await this.restClient.Put(ct, uri, parameters, Headers(accessToken), null);

            if (statusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(statusCode);
            }

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<IDictionary<string, string>> GetPlaylistNames(string accessToken, CancellationToken ct)
        {
            var playlists = await this.ListAllPlaylists(accessToken, ct);

            return playlists.ToDictionary(p => p.Id, p => p.Name);
        }

        public async Task<IDictionary<string, string>> GetDeviceSourceNames(string accessToken, string deviceId, CancellationToken ct)
        {
            var player = await this.ListPlayer(accessToken, deviceId, ct);

            return player.Sources.Where(s => s.Visible).ToDictionary(s => s.Id, s => s.Name);
        }

        public async Task<string> SelectSource(string accessToken, string deviceId, string sourceId, CancellationToken ct)
        {
            var uri = new Uri($"{this.apiRoot}/players/{deviceId}/source");

            var parameters = new Dictionary<string, string> { { "sourceId", sourceId } };

            var statusCode = await this.restClient.Put(ct, uri, parameters, Headers(accessToken), null);

            if (statusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(statusCode);
            }

            return DateTime.UtcNow.ToString("o");
        }

        private static Dictionary<string, string[]> Headers(string accessToken)
        {
            var headers = new Dictionary<string, string[]> { ["Authorization"] = new[] { $"Bearer {accessToken}" } };
            return headers;
        }

        private async Task<PlayerResource> ListPlayer(string accessToken, string deviceId, CancellationToken ct)
        {
            var playerResponse = await this.restClient.Get<PlayerResource>(ct, new Uri($"{this.apiRoot}/players/{deviceId}"), null, Headers(accessToken));

            if (playerResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(playerResponse.StatusCode);
            }

            return playerResponse.Value;
        }

        private async Task<PlayerResource[]> ListAllPlayers(string accessToken, CancellationToken ct)
        {
            var playersResponse = await this.restClient.Get<PlayerResource[]>(ct, new Uri($"{this.apiRoot}/players/"), null, Headers(accessToken));

            if (playersResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(playersResponse.StatusCode);
            }

            return playersResponse.Value;
        }

        private async Task<PlaylistResource[]> ListAllPlaylists(string accessToken, CancellationToken ct)
        {
            var queryParameters = new Dictionary<string, string> { { "sortBy", "name" }, { "includeOnly", "named" } };

            var playlistResponse = await this.restClient.Get<PlaylistResource[]>(ct, new Uri($"{this.apiRoot}/playlists/"), queryParameters, Headers(accessToken));

            if (playlistResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(playlistResponse.StatusCode);
            }

            return playlistResponse.Value;
        }
    }
}