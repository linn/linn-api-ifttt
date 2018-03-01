namespace Linn.Api.Ifttt.Service
{
    using System.Linq;

    using Microsoft.AspNetCore.Http;

    public static class HttpRequestExtensions
    {
        public static string GetAccessToken(this HttpRequest req)
        {
            var authorizationHeader =
               req.Headers["Authorization"].FirstOrDefault(h => h.StartsWith("Bearer "));

            return authorizationHeader?.Substring(7);
        }
    }
}
