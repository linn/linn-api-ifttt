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

        public async Task<string> TurnOfAllDevices(string accessToken, CancellationToken ct)
        {
            var headers = new Dictionary<string, string[]> { ["Authorization"] = new[] { $"Bearer {accessToken}" } };
            var playersResponse = await this.restClient.Get<PlayerResource[]>(ct, new Uri($"{this.apiRoot}/players/"), null, headers);

            if (playersResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            await Task.WhenAll(
                playersResponse.Value.Select(p => p.Links.FirstOrDefault(l => l.Rel == "standby")?.Href)
                    .Where(s => !string.IsNullOrEmpty(s)).Select(
                        href => this.restClient.Put(ct, new Uri($"{this.apiRoot}{href}"), null, headers, null)));

            return DateTime.UtcNow.ToString("o");
        }
    }
}