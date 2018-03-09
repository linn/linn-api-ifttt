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

    public class WhenUnmutingASpecificDeviceWithMissingDeviceId : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        public WhenUnmutingASpecificDeviceWithMissingDeviceId()
        {
            var deviceId = Guid.NewGuid().ToString();

            var request = new
                              {
                                  actionFields = new { },
                                  ifttt_source = new { id = "2", url = "https://ifttt.com/myrecipes/personal/2" },
                                  user = new { timezone = "Pacific Time (US & Canada)" }
                              };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.LinnApiActions.MuteDevice(this.AccessToken, deviceId, Arg.Any<CancellationToken>()).Returns("id");

            this.Client.SetAccessToken(this.AccessToken);

            this.response = this.Client.PostAsync("/ifttt/v1/actions/unmute_device", content).Result;

            this.result = this.response.JsonBody<ErrorResource>();
        }

        [Fact]
        public void ShouldReturnBadRequest()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void ShouldDescribeError()
        {
            this.result.Errors.Should().HaveCount(1);
            this.result.Errors[0].Message.Should().Be("Action field `device_id` missing");
        }
    }
}
