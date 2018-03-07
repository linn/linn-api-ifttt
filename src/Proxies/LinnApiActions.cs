namespace Linn.Api.Ifttt.Proxies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

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

            if (statusCodes.Contains(HttpStatusCode.Forbidden))
            {
                throw new Exception($"Linn API status codes: [{string.Join(",", statusCodes)}]");
            }

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<string> TurnOffDevice(string accessToken, string deviceId, CancellationToken ct)
        {
            var uri = new Uri($"{this.apiRoot}/devices/{deviceId}/standby");

            var statusCode = await this.restClient.Put(ct, uri, null, Headers(accessToken), null);

            if (statusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception($"Linn API status code: {statusCode}");
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

            var parameters = new Dictionary<string, string> { { "url", mediaUri }, { "title", mediaTitle }, { "artworkUrl", mediaArtworkUrl } };

            var statusCode = await this.restClient.Put(ct, uri, parameters, Headers(accessToken), null);

            if (statusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception($"Linn API status code: {statusCode}");
            }

            return DateTime.UtcNow.ToString("o");
        }

        private static Dictionary<string, string[]> Headers(string accessToken)
        {
            var headers = new Dictionary<string, string[]> { ["Authorization"] = new[] { $"Bearer {accessToken}" } };
            return headers;
        }

        private async Task<PlayerResource[]> ListAllPlayers(string accessToken, CancellationToken ct)
        {
            var playersResponse = await this.restClient.Get<PlayerResource[]>(ct, new Uri($"{this.apiRoot}/players/"), null, Headers(accessToken));

            if (playersResponse.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception($"Linn API status code: {playersResponse.StatusCode}");
            }

            return playersResponse.Value;
        }
    }
}