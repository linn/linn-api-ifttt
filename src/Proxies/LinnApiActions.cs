namespace Linn.Api.Ifttt.Proxies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Api.Ifttt.Resources.Linn;
    using Linn.Common.Proxy;

    public class LinnApiActions : ILinnApiActions
    {
        private readonly IRestClient restClient;

        private readonly string apiRoot;

        public LinnApiActions(IRestClient restClient, string apiRoot)
        {
            this.restClient = restClient;
            this.apiRoot = apiRoot;
        }

        public async Task<string> TurnOffAllDevices(string accessToken, CancellationToken ct)
        {
            var headers = new Dictionary<string, string[]> { ["Authorization"] = new[] { $"Bearer {accessToken}" } };

            var players = await this.ListAllPlayers(accessToken, ct);

            var tasks = players
                .Select(p => p.Links.FirstOrDefault(l => l.Rel == "standby")?.Href)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(href => new Uri($"{this.apiRoot}{href}"))
                .Select(uri => this.restClient.Put(ct, uri, null, headers, null));

            await Task.WhenAll(tasks);

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<string> TurnOffDevice(string accessToken, string deviceId, CancellationToken ct)
        {
            var headers = new Dictionary<string, string[]> { ["Authorization"] = new[] { $"Bearer {accessToken}" } };

            var uri = new Uri($"{this.apiRoot}/devices/{deviceId}/standby");

            await this.restClient.Put(ct, uri, null, headers, null);

            return DateTime.UtcNow.ToString("o");
        }

        public async Task<IDictionary<string, string>> FindAllDevices(string accessToken, CancellationToken ct)
        {
            var players = await this.ListAllPlayers(accessToken, ct);

            return players.ToDictionary(p => p.Id, p => p.Name);
        }

        private async Task<PlayerResource[]> ListAllPlayers(string accessToken, CancellationToken ct)
        {
            var headers = new Dictionary<string, string[]> { ["Authorization"] = new[] { $"Bearer {accessToken}" } };

            var playersResponse = await this.restClient.Get<PlayerResource[]>(ct, new Uri($"{this.apiRoot}/players/"), null, headers);

            return playersResponse.StatusCode != HttpStatusCode.OK ? null : playersResponse.Value;
        }
    }
}