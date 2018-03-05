namespace Linn.Api.Ifttt.Service.Modules
{
    using System.Threading.Tasks;

    using Botwin;

    public class ServiceStatusModule : BotwinModule
    {
        public ServiceStatusModule()
        {
            this.Get("/ifttt/v1/status", (req, res, routeData) => Task.CompletedTask);
        }
    }
}
