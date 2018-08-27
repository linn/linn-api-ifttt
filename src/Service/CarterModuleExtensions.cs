namespace Linn.Api.Ifttt.Service
{
    using System.Net;
    using System.Threading.Tasks;

    using Carter;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Common.Configuration;

    public static class CarterModuleExtensions
    {
        public static void RequiresAccessToken(this CarterModule module)
        {
            module.Before += ctx =>
                {
                    if (ctx.Request.GetAccessToken() != null)
                    {
                        return Task.FromResult(true);
                    }

                    throw new LinnApiException(HttpStatusCode.Unauthorized);
                };
        }

        public static void RequiresIftttServiceKey(this CarterModule module)
        {
            module.Before = ctx =>
                {
                    if (ctx.Request.Headers.TryGetValue("IFTTT-Service-Key", out var serviceKey) && serviceKey == ConfigurationManager.Configuration["iftttServiceKey"])
                    {
                        return Task.FromResult(true);
                    }

                    throw new InvalidServiceKeyException();
                };
        }
    }
}