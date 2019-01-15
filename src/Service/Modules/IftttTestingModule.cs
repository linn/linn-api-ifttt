namespace Linn.Api.Ifttt.Service.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using IdentityModel.Client;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Ifttt;
    using Linn.Api.Ifttt.Resources.Ifttt.Testing;
    using Linn.Common.Configuration;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Newtonsoft.Json;

    public class IftttTestingModule : CarterModule
    {
        private readonly ILinnApiActions actions;

        public IftttTestingModule(ILinnApiActions actions)
        {
            this.actions = actions;
            this.RequiresIftttServiceKey();

            this.Post("/ifttt/v1/test/setup", this.GenerateTestData);
        }

        private static TestSamples GenerateSamples(IEnumerable<string> deviceIds, IEnumerable<string> playlistIds, IEnumerable<string> sourceIds)
        {
            var deviceId = deviceIds.FirstOrDefault();
            var playlistId = playlistIds.FirstOrDefault();
            var sourceId = sourceIds.FirstOrDefault();

            return new TestSamples
                       {
                           Triggers = new TriggerSamples(),
                           Actions =
                               new ActionSamples
                                   {
                                       turn_off_all_devices = TurnOffAllDevicesSampleData(),
                                       turn_off_device = DeviceSampleData(deviceId),
                                       play_single_media =
                                           PlaySingleMediaSampleData(deviceId),
                                       play_playlist = PlayPlaylistSampleData(
                                           deviceId,
                                           playlistId),
                                       mute_device = DeviceSampleData(deviceId),
                                       unmute_device = DeviceSampleData(deviceId),
                                       invoke_pin = PlayPinSampleData(deviceId),
                                       select_source = PlaySourceSampleData(deviceId, sourceId),
                               },
                           ActionRecordSkipping = new ActionSamples
                                                      {
                                                          turn_off_device =
                                                              DeviceSampleData(
                                                                  "UNKNOWN_DEVICE"),
                                                          play_single_media =
                                                              PlaySingleMediaSampleData(
                                                                  "UNKNOWN_DEVICE"),
                                                          play_playlist =
                                                              PlayPlaylistSampleData(
                                                                  deviceId,
                                                                  "UNKNOWN_PLAYLIST"),
                                                          mute_device = DeviceSampleData("UNKNOWN_DEVICE"),
                                                          unmute_device = DeviceSampleData("UNKNOWN_DEVICE"),
                                                          invoke_pin = PlayPinSampleData("UNKNOWN_DEVICE"),
                                                          select_source = PlaySourceSampleData(deviceId, "UNKNOWN SOURCE"),
                                                      }
                       };
        }

        private static Dictionary<string, string> TurnOffAllDevicesSampleData()
        {
            return new Dictionary<string, string>();
        }

        private static Dictionary<string, string> PlayPlaylistSampleData(string deviceId, string playlistId)
        {
            return new Dictionary<string, string>
                       {
                           { "device_id", deviceId },
                           { "playlist_id", playlistId }
                       };
        }

        private static Dictionary<string, string> PlaySingleMediaSampleData(string deviceId)
        {
            return new Dictionary<string, string>
                       {
                           { "device_id", deviceId },
                           { "media_url", "http://a.files.bbci.co.uk/media/live/manifesto/audio/simulcast/hls/uk/sbr_high/ak/bbc_6music.m3u8" },
                           { "media_title", "Test Title" },
                           { "media_artwork_url", "http://cdn-images.deezer.com/images/playlist/1f26eadbca956fe29bc7c58d1048ee23/500x500.jpg" },
                       };
        }

        private static Dictionary<string, string> DeviceSampleData(string deviceId)
        {
            return new Dictionary<string, string>
                       {
                           { "device_id", deviceId }
                       };
        }

        private static Dictionary<string, string> PlayPinSampleData(string deviceId)
        {
            return new Dictionary<string, string>
                       {
                           { "device_id", deviceId },
                           { "pin_id", "3" }
                       };
        }

        private static Dictionary<string, string> PlaySourceSampleData(string deviceId, string sourceId)
        {
            return new Dictionary<string, string>
                       {
                           { "devicesource_id", JsonConvert.SerializeObject(new SelectSourceActionFieldResource.DeviceSourceResource { DeviceId = deviceId, SourceId = sourceId }) }
                       };
        }

        private static async Task<string> AccessToken()
        {
            var disco = await DiscoveryClient.GetAsync(ConfigurationManager.Configuration["discoveryEndpoint"]);

            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var client = new TokenClient(
                disco.TokenEndpoint,
                ConfigurationManager.Configuration["testClientId"],
                ConfigurationManager.Configuration["testClientSecret"]);

            var response = await client.RequestResourceOwnerPasswordAsync(
                       ConfigurationManager.Configuration["testUsername"],
                       ConfigurationManager.Configuration["testPassword"],
                       "openid email offline_access device_control volume_control player_information list_playlists read_playlists");

            return response.AccessToken;
        }

        private async Task GenerateTestData(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            var accessToken = await AccessToken();

            var devices = await this.actions.GetDeviceNames(accessToken, res.HttpContext.RequestAborted);

            var playlists = await this.actions.GetPlaylistNames(accessToken, res.HttpContext.RequestAborted);

            var tasks = devices.Select(d => this.actions.GetDeviceSourceNames(accessToken, d.Key, res.HttpContext.RequestAborted));

            var results = await Task.WhenAll(tasks.ToArray());

            var sources = results.SelectMany(r => r.Keys);

            var testData = new TestDataResource
                               {
                                   AccessToken = accessToken,
                                   Samples = GenerateSamples(devices.Keys, playlists.Keys, sources)
                               };

            await res.AsJson(new DataResource<TestDataResource>(testData));
        }
    }
}
