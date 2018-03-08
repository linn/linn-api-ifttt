namespace Linn.Api.Ifttt.Testing.Integration
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public static class HttpClientExtensions
    {
        public static void SetAccessToken(this HttpClient client, string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        public static Task<HttpResponseMessage> Post<T>(this HttpClient client, string uri, T request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request));

            return client.PostAsync(uri, content);
        }
    }
}
