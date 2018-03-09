namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources.Ifttt;

    using Newtonsoft.Json;

    using NSubstitute;

    using Xunit;

    public class WhenConfiguringDeviceIdForMutingASpecificDevice : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly DataResource<ActionFieldOption[]> result;

        private readonly Dictionary<string, string> devices;

        public WhenConfiguringDeviceIdForMutingASpecificDevice()
        {
            var request = new { };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.devices = new Dictionary<string, string>
                              {
                                  [Guid.NewGuid().ToString()] = "Kitchen",
                                  [Guid.NewGuid().ToString()] = "Summer Room",
                                  [Guid.NewGuid().ToString()] = "Winter Room"
                              };

            this.LinnApiActions.GetDeviceNames(this.AccessToken, Arg.Any<CancellationToken>()).Returns(this.devices);

            this.Client.SetAccessToken(this.AccessToken);

            this.response = this.Client.PostAsync("/ifttt/v1/actions/mute_device/fields/device_id/options", content).Result;

            this.result = this.response.JsonBody<DataResource<ActionFieldOption[]>>();
        }

        [Fact]
        public void ShouldReturnOk()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public void ShouldReturnData()
        {
            this.result.Data.Should().HaveCount(this.devices.Count);
            foreach (var device in this.devices)
            {
                this.result.Data.First(d => d.Value == device.Key).Label.Should().Be(device.Value);
            }
        }
    }
}
