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

            await Task.WhenAll(tasks);

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<string> TurnOffDevice(string accessToken, string deviceId, CancellationToken ct)
        {
            var uri = new Uri($"{this.apiRoot}/devices/{deviceId}/standby");

            await this.restClient.Put(ct, uri, null, Headers(accessToken), null);

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<IDictionary<string, string>> GetDeviceNames(string accessToken, CancellationToken ct)
        {
            var players = await this.ListAllPlayers(accessToken, ct);

            return players.ToDictionary(p => p.Id, p => p.Name);
        }

        private static Dictionary<string, string[]> Headers(string accessToken)
        {
            var headers = new Dictionary<string, string[]> { ["Authorization"] = new[] { $"Bearer {accessToken}" } };
            return headers;
        }

        private async Task<PlayerResource[]> ListAllPlayers(string accessToken, CancellationToken ct)
        {
            var playersResponse = await this.restClient.Get<PlayerResource[]>(ct, new Uri($"{this.apiRoot}/players/"), null, Headers(accessToken));

            return playersResponse.StatusCode != HttpStatusCode.OK ? null : playersResponse.Value;
        }
    }
}