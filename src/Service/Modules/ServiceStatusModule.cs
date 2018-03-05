namespace Linn.Api.Ifttt.Service.Modules
{
    using System.Threading.Tasks;

    using Botwin;

    using Linn.Common.Configuration;

    public class ServiceStatusModule : BotwinModule
    {
        public ServiceStatusModule()
        {
            this.Before = ctx =>
                {
                    ctx.Request.Headers.TryGetValue("IFTTT-Service-Key", out var serviceKey);

                    if (serviceKey == ConfigurationManager.Configuration["iftttServiceKey"])
                    {
                        return Task.FromResult(false);
                    }

                    ctx.Response.StatusCode = 401;

                    return Task.FromResult(true);
                };

            this.Get("/ifttt/v1/status", (req, res, routeData) => Task.CompletedTask);
        }
    }
}
