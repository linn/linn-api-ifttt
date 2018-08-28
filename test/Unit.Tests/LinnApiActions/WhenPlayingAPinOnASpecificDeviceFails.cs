namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Proxies;

    using NSubstitute;

    using Xunit;

    public class WhenPlayingAPinOnASpecificDeviceFails : ContextBase
    {
        private readonly HttpStatusCode statusCode;

        private readonly Action action;

        public WhenPlayingAPinOnASpecificDeviceFails()
        {
            this.statusCode = HttpStatusCode.Forbidden;

            var accessToken = Guid.NewGuid().ToString();

            var deviceId = Guid.NewGuid().ToString();

            var pinId = "3";

            this.RestClient.Put(
                Arg.Any<CancellationToken>(),
                Arg.Any<Uri>(),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<Dictionary<string, string[]>>(),
                Arg.Any<object>()).Returns(this.statusCode);

            this.action = () => this.Sut.InvokePin(accessToken, deviceId, pinId, CancellationToken.None).Wait();
        }

        [Fact]
        public void ShouldThrowLinnApiException()
        {
            this.action.Should().Throw<LinnApiException>().And.StatusCode.Should().Be(this.statusCode);
        }
    }
}
