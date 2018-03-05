namespace Linn.Api.Ifttt.Service
{
    using System.Threading.Tasks;

    using Botwin;

    using Linn.Common.Configuration;

    public static class BotwinModuleExtensions
    {
        public static void RequiresAccessToken(this BotwinModule module)
        {
            module.Before += ctx =>
                {
                    var accessToken = ctx.Request.GetAccessToken();

                    if (accessToken != null)
                    {
                        return Task.FromResult(true);
                    }

                    ctx.Response.StatusCode = 401;

                    return Task.FromResult(false);
                };
        }

        public static void RequiresIftttServiceKey(this BotwinModule module)
        {
            module.Before = ctx =>
                {
                    ctx.Request.Headers.TryGetValue("IFTTT-Service-Key", out var serviceKey);

                    if (serviceKey == ConfigurationManager.Configuration["iftttServiceKey"])
                    {
                        return Task.FromResult(true);
                    }

                    ctx.Response.StatusCode = 401;

                    return Task.FromResult(false);
                };
        }
    }
}