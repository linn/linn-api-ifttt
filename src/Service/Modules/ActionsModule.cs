namespace Linn.Api.Ifttt.Service.Modules
{
    using System.Linq;

    using Botwin;
    using Botwin.ModelBinding;
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
                        var id = await linnApiProxy.TurnOffAllDevices(req.GetAccessToken(), req.HttpContext.RequestAborted);
                        var resource = new[] { new ActionResponseResource { Id = id } };
                        await res.AsJson(new DataResource<ActionResponseResource[]>(resource));
                    });

            this.Post(
                "/ifttt/v1/actions/turn_off_device",
                async (req, res, routeData) =>
                    {
                        var model = req.Bind<ActionRequestResource>();
                        var id = await linnApiProxy.TurnOffDevice(req.GetAccessToken(), model.ActionFields["device_id"], req.HttpContext.RequestAborted);
                        var resource = new[] { new ActionResponseResource { Id = id } };
                        await res.AsJson(new DataResource<ActionResponseResource[]>(resource));
                    });

            this.Post(
                "/ifttt/v1/actions/turn_off_device/fields/device_id/options",
                async (req, res, routeData) =>
                    {
                        var players = await linnApiProxy.FindAllDevices(req.GetAccessToken(), req.HttpContext.RequestAborted);
                        var resource = players.Select(p => new ActionFieldOption { Label = p.Value, Value = p.Key }).ToArray();
                        await res.AsJson(new DataResource<ActionFieldOption[]>(resource));
                    });
        }
    }
}
