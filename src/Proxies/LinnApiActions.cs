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

    using Newtonsoft.Json;

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

            var tasks = playersResponse.Value
                .Select(p => p.Links.FirstOrDefault(l => l.Rel == "standby")?.Href)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(href => new Uri($"{this.apiRoot}{href}"))
                .Select(uri => this.restClient.Put(ct, uri, null, headers, null));

            await Task.WhenAll(tasks);

            return DateTime.UtcNow.ToString("o");
        }
    }
}