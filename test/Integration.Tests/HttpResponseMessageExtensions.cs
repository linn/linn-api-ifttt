namespace Linn.Api.Ifttt.Testing.Integration
{
    using System.Net.Http;

    using Newtonsoft.Json;

    public static class HttpResponseMessageExtensions
    {
        public static T JsonBody<T>(this HttpResponseMessage res)
        {
            return JsonConvert.DeserializeObject<T>(
                res.Content.ReadAsStringAsync().Result);
        }
    }
}
