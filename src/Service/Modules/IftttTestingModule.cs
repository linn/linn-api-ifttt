namespace Linn.Api.Ifttt.Service.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Botwin;
    using Botwin.Response;

    using IdentityModel.Client;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Ifttt;
    using Linn.Api.Ifttt.Resources.Ifttt.Testing;
    using Linn.Common.Configuration;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class IftttTestingModule : BotwinModule
    {
        private readonly ILinnApiActions actions;

        public IftttTestingModule(ILinnApiActions actions)
        {
            this.actions = actions;
            this.RequiresIftttServiceKey();

            this.Post("/ifttt/v1/test/setup", this.GenerateTestData);
        }

        private static TestSamples GenerateSamples(string deviceId)
        {
            return new TestSamples
                       {
                           Triggers = new TriggerSamples(),
                           Actions = new ActionSamples
                                         {
                                             turn_off_all_devices = TurnOffAllDevicesSampleData(),
                                             turn_off_device = TurnOffDeviceSampleData(deviceId),
                                             play_single_media = PlaySingleMediaSampleData(deviceId),
                                         }
                       };
        }

        private static Dictionary<string, string> TurnOffAllDevicesSampleData()
        {
            return new Dictionary<string, string>();
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

        private static Dictionary<string, string> TurnOffDeviceSampleData(string deviceId)
        {
            return new Dictionary<string, string>
                       {
                           { "device_id", deviceId }
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
                       "openid email offline_access device_control volume_control player_information read_playlists");

            return response.AccessToken;
        }

        private async Task GenerateTestData(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            var accessToken = await AccessToken();

            var devices = await this.actions.GetDeviceNames(accessToken, res.HttpContext.RequestAborted);

            var testData = new TestDataResource
                               {
                                   AccessToken = accessToken,
                                   Samples = GenerateSamples(devices.Keys.First())
                               };

            await res.AsJson(new DataResource<TestDataResource>(testData));
        }
    }
}
