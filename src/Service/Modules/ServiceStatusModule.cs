namespace Linn.Api.Ifttt.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;

    public class ServiceStatusModule : CarterModule
    {
        public ServiceStatusModule()
        {
            this.RequiresIftttServiceKey();

            this.Get("/ifttt/v1/status", (req, res, routeData) => Task.CompletedTask);
        }
    }
}
