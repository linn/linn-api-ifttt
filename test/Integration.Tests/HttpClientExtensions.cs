namespace Linn.Api.Ifttt.Testing.Integration
{
    using System.Net.Http;
    using System.Net.Http.Headers;

    public static class HttpClientExtensions
    {
        public static void SetAccessToken(this HttpClient client, string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }
}
