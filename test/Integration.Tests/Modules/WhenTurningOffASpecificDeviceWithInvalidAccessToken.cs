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
    using NSubstitute.ExceptionExtensions;

    using Xunit;

    public class WhenTurningOffASpecificDeviceWithInvalidAccessToken : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        private readonly string errorMessage;

        public WhenTurningOffASpecificDeviceWithInvalidAccessToken()
        {
            this.errorMessage = "Failure";

            var deviceId = Guid.NewGuid().ToString();

            var request = new
                              {
                                  actionFields = new { device_id = deviceId },
                                  ifttt_source = new { id = "2", url = "https://ifttt.com/myrecipes/personal/2" },
                                  user = new { timezone = "Pacific Time (US & Canada)" }
                              };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.LinnApiActions.TurnOffDevice(Arg.Any<string>(), deviceId, Arg.Any<CancellationToken>()).Throws(new Exception(this.errorMessage));

            this.Client.SetAccessToken(Guid.NewGuid().ToString());

            this.response = this.Client.PostAsync("/ifttt/v1/actions/turn_off_device", content).Result;

            this.result = this.response.JsonBody<ErrorResource>();
        }

        [Fact]
        public void ShouldReturnForbidden()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public void ShouldReturnBody()
        {
            this.result.Errors.Should().HaveCount(1);
            this.result.Errors[0].Message.Should().Be(this.errorMessage);
        }
    }
}