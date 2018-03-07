namespace Linn.Api.Ifttt.Service.Modules
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Botwin;
    using Botwin.ModelBinding;
    using Botwin.Response;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Ifttt;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ActionsModule : BotwinModule
    {
        private readonly ILinnApiActions linnApiProxy;

        public ActionsModule(ILinnApiActions linnApiProxy)
        {
            this.linnApiProxy = linnApiProxy;

            this.RequiresAccessToken();

            this.Post("/ifttt/v1/actions/turn_off_all_devices", this.TurnOffAllDevices);
            this.Post("/ifttt/v1/actions/turn_off_device", this.TurnOffDevice);
            this.Post("/ifttt/v1/actions/turn_off_device/fields/device_id/options", this.ListDevices);
            this.Post("/ifttt/v1/actions/play_single_media", this.PlaySingleMedia);
            this.Post("/ifttt/v1/actions/play_single_media/fields/device_id/options", this.ListDevices);
            this.Post("/ifttt/v1/actions/play_playlist", this.PlayPlaylist);
            this.Post("/ifttt/v1/actions/play_playlist/fields/device_id/options", this.ListDevices);
            this.Post("/ifttt/v1/actions/play_playlist/fields/playlist_id/options", this.ListPlaylists);
        }

        private async Task TurnOffAllDevices(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            try
            {
                var actionResponseId = await this.linnApiProxy.TurnOffAllDevices(
                                           req.GetAccessToken(),
                                           req.HttpContext.RequestAborted);

                var resource = new[] { new ActionResponse { Id = actionResponseId } };

                await res.AsJson(new DataResource<ActionResponse[]>(resource), req.HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                res.StatusCode = 401;

                await res.AsJson(new ErrorResource(e.Message), req.HttpContext.RequestAborted);
            }
        }

        private async Task TurnOffDevice(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            var model = req.BindAndValidate<ActionRequestResource<DeviceActionFieldResource>>();

            if (model.ValidationResult.IsValid)
            {
                try
                {
                    var actionResponseId = await this.linnApiProxy.TurnOffDevice(
                                               req.GetAccessToken(),
                                               model.Data.ActionFields.Device_Id,
                                               req.HttpContext.RequestAborted);

                    var resource = new[] { new ActionResponse { Id = actionResponseId } };

                    await res.AsJson(new DataResource<ActionResponse[]>(resource), req.HttpContext.RequestAborted);
                }
                catch (Exception e)
                {
                    res.StatusCode = 401;

                    await res.AsJson(new ErrorResource(e.Message), req.HttpContext.RequestAborted);
                }
            }
            else
            {
                res.StatusCode = 400;

                var errorMessages = model.ValidationResult.Errors.Select(e => new ErrorMessage(e.ErrorMessage)).ToArray();

                await res.AsJson(new ErrorResource { Errors = errorMessages });
            }
        }

        private async Task PlayPlaylist(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            var model = req.BindAndValidate<ActionRequestResource<PlayPlaylistActionFieldResource>>();

            if (model.ValidationResult.IsValid)
            {
                try
                {
                    var actionResponseId = await this.linnApiProxy.PlayPlaylist(
                                               req.GetAccessToken(),
                                               model.Data.ActionFields.Device_Id,
                                               model.Data.ActionFields.Playlist_Id,
                                               req.HttpContext.RequestAborted);

                    var resource = new[] { new ActionResponse { Id = actionResponseId } };

                    await res.AsJson(new DataResource<ActionResponse[]>(resource), req.HttpContext.RequestAborted);
                }
                catch (Exception e)
                {
                    res.StatusCode = 401;

                    await res.AsJson(new ErrorResource(e.Message), req.HttpContext.RequestAborted);
                }
            }
            else
            {
                res.StatusCode = 400;

                var errorMessages = model.ValidationResult.Errors.Select(e => new ErrorMessage(e.ErrorMessage)).ToArray();

                await res.AsJson(new ErrorResource { Errors = errorMessages });
            }
        }

        private async Task PlaySingleMedia(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            var model = req.BindAndValidate<ActionRequestResource<PlayMediaItemActionFieldResource>>();

            if (model.ValidationResult.IsValid)
            {
                try
                {
                    var actionResponseId = await this.linnApiProxy.PlaySingleMedia(
                                               req.GetAccessToken(),
                                               model.Data.ActionFields.Device_Id,
                                               model.Data.ActionFields.Media_Url,
                                               model.Data.ActionFields.Media_Title,
                                               model.Data.ActionFields.Media_Artwork_Url,
                                               req.HttpContext.RequestAborted);

                    var resource = new[] { new ActionResponse { Id = actionResponseId } };

                    await res.AsJson(new DataResource<ActionResponse[]>(resource), req.HttpContext.RequestAborted);
                }
                catch (Exception e)
                {
                    res.StatusCode = 401;

                    await res.AsJson(new ErrorResource(e.Message), req.HttpContext.RequestAborted);
                }
            }
            else
            {
                res.StatusCode = 400;

                var errorMessages = model.ValidationResult.Errors.Select(e => new ErrorMessage(e.ErrorMessage)).ToArray();

                await res.AsJson(new ErrorResource { Errors = errorMessages });
            }
        }

        private async Task ListPlaylists(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            try
            {
                var playlists = await this.linnApiProxy.GetPlaylistNames(
                                  req.GetAccessToken(),
                                  req.HttpContext.RequestAborted);

                var actionFieldOptions = playlists.Select(p => new ActionFieldOption { Label = p.Value, Value = p.Key })
                    .ToArray();

                await res.AsJson(
                    new DataResource<ActionFieldOption[]>(actionFieldOptions),
                    req.HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                res.StatusCode = 401;

                await res.AsJson(new ErrorResource(e.Message), req.HttpContext.RequestAborted);
            }
        }

        private async Task ListDevices(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            try
            {
                var players = await this.linnApiProxy.GetDeviceNames(
                                  req.GetAccessToken(),
                                  req.HttpContext.RequestAborted);

                var actionFieldOptions = players.Select(p => new ActionFieldOption { Label = p.Value, Value = p.Key })
                    .ToArray();

                await res.AsJson(
                    new DataResource<ActionFieldOption[]>(actionFieldOptions),
                    req.HttpContext.RequestAborted);
            }
            catch (Exception e)
            {
                res.StatusCode = 401;

                await res.AsJson(new ErrorResource(e.Message), req.HttpContext.RequestAborted);
            }
        }
    }
}
