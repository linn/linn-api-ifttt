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

    public class WhenConfiguringDeviceIdForTurningOffASpecificDeviceWithInvalidAccessToken : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        public WhenConfiguringDeviceIdForTurningOffASpecificDeviceWithInvalidAccessToken()
        {
            var request = new { };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.LinnApiActions.GetDeviceNames(Arg.Any<string>(), Arg.Any<CancellationToken>()).Throws(new LinnApiException(HttpStatusCode.Forbidden));

            this.Client.SetAccessToken(Guid.NewGuid().ToString());

            this.response = this.Client.PostAsync("/ifttt/v1/actions/turn_off_device/fields/device_id/options", content).Result;

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
            this.result.Errors[0].Message.Should().Be("Linn API status code: Forbidden");
        }
    }
}
