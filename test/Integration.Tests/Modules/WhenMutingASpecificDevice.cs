namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources.Ifttt;

    using Newtonsoft.Json;

    using NSubstitute;

    using Xunit;

    public class WhenMutingASpecificDevice : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly DataResource<ActionResponse[]> result;

        public WhenMutingASpecificDevice()
        {
            var deviceId = Guid.NewGuid().ToString();

            var request = new
                              {
                                  actionFields = new { device_id = deviceId },
                                  ifttt_source = new { id = "2", url = "https://ifttt.com/myrecipes/personal/2" },
                                  user = new { timezone = "Pacific Time (US & Canada)" }
                              };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.LinnApiActions.MuteDevice(this.AccessToken, deviceId, Arg.Any<CancellationToken>()).Returns("id");

            this.Client.SetAccessToken(this.AccessToken);

            this.response = this.Client.PostAsync("/ifttt/v1/actions/mute_device", content).Result;

            this.result = this.response.JsonBody<DataResource<ActionResponse[]>>();
        }

        [Fact]
        public void ShouldReturnOk()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public void ShouldReturnSomethingIntheId()
        {
            this.result.Data[0].Id.Should().NotBeNullOrEmpty();
        }
    }
}
