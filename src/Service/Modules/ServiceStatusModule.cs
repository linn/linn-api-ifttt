namespace Linn.Api.Ifttt.Service.Modules
{
    using Botwin;
    using Botwin.Response;

    public class ServiceStatusModule : BotwinModule
    {
        public ServiceStatusModule()
        {
            this.RequiresIftttServiceKey();

            this.Get(
                "/ifttt/v1/status",
                (req, res, routeData) =>
                    {
                        res.StatusCode = 200;
                        return res.AsJson(new { });
                    });
        }
    }
}
