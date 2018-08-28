namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Ifttt;

    using Newtonsoft.Json;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using Xunit;

    public class WhenInvokePinOnASpecificDeviceWithBadlyConfiguredPin : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        public WhenInvokePinOnASpecificDeviceWithBadlyConfiguredPin()
        {
            var deviceId = Guid.NewGuid().ToString();

            var pinId = Guid.NewGuid().ToString();

            var request = new
                              {
                                  actionFields = new { device_id = deviceId, pin_id = pinId },
                                  ifttt_source = new { id = "2", url = "https://ifttt.com/myrecipes/personal/2" },
                                  user = new { timezone = "Pacific Time (US & Canada)" }
                              };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.LinnApiActions.InvokePin(Arg.Any<string>(), deviceId, pinId, Arg.Any<CancellationToken>())
                .Throws(new LinnApiException(HttpStatusCode.BadRequest));

            this.Client.SetAccessToken(Guid.NewGuid().ToString());

            this.response = this.Client.PostAsync("/ifttt/v1/actions/invoke_pin", content).Result;

            this.result = this.response.JsonBody<ErrorResource>();
        }

        [Fact]
        public void ShouldReturnBadRequest()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void ShouldReturnSkipMessage()
        {
            this.result.Errors.Should().HaveCount(1);
            this.result.Errors[0].Status.Should().Be("SKIP");
            this.result.Errors[0].Message.Should().Be("Linn API status code: BadRequest");
        }
    }
}
