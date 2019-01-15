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

namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    public class WhenSelectSourceOnASpecificDeviceWithInvalidAccessToken : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        public WhenSelectSourceOnASpecificDeviceWithInvalidAccessToken()
        {
            var deviceId = Guid.NewGuid().ToString();

            var sourceId = Guid.NewGuid().ToString();

            var request = new
            {
                actionFields = new { devicesource_id = JsonConvert.SerializeObject(new { DeviceId = deviceId, SourceId = sourceId }) },
                ifttt_source = new { id = "2", url = "https://ifttt.com/myrecipes/personal/2" },
                user = new { timezone = "Pacific Time (US & Canada)" }
            };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.LinnApiActions.SelectSource(Arg.Any<string>(), deviceId, sourceId, Arg.Any<CancellationToken>())
                .Throws(new LinnApiException(HttpStatusCode.Forbidden));

            this.Client.SetAccessToken(Guid.NewGuid().ToString());

            this.response = this.Client.PostAsync("/ifttt/v1/actions/select_source", content).Result;

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
