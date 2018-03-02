namespace Linn.Api.Ifttt.Service.Modules
{
    using System.Threading;

    using Botwin;
    using Botwin.Response;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Ifttt;

    public class ActionsModule : BotwinModule
    {
        public ActionsModule(ILinnApiActions linnApiProxy)
        {
            this.Post(
                "/ifttt/v1/actions/turn_off_all_devices",
                async (req, res, routeData) =>
                    {
                        var id = await linnApiProxy.TurnOfAllDevices(req.GetAccessToken(), CancellationToken.None);
                        var resource = new[] { new ActionResponseResource { Id = id } };
                        await res.AsJson(new DataResource<ActionResponseResource[]>(resource));
                    });
        }
    }
}
