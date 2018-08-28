namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    using FluentAssertions;

    using NSubstitute;

    using Xunit;

    public class WhenPlayingAPinOnASpecificDevice : ContextBase
    {
        private readonly string accessToken;

        private readonly string deviceId;

        private readonly string pinId;

        private readonly string result;

        public WhenPlayingAPinOnASpecificDevice()
        {
            this.accessToken = Guid.NewGuid().ToString();

            this.deviceId = Guid.NewGuid().ToString();

            this.pinId = "3";

            this.RestClient.Put(
                Arg.Any<CancellationToken>(),
                Arg.Any<Uri>(),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<Dictionary<string, string[]>>(),
                Arg.Any<object>()).Returns(HttpStatusCode.OK);

            this.result = this.Sut.InvokePin(this.accessToken, this.deviceId, this.pinId, CancellationToken.None).Result;
        }

        [Fact]
        public void ShouldReturnATimestamp()
        {
            DateTime.Parse(this.result).ToUniversalTime().Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void ShouldSendAPlay()
        {
            this.RestClient.Received().Put(
                Arg.Any<CancellationToken>(),
                Arg.Is<Uri>(uri => uri.ToString() == "http://localhost/players/" + this.deviceId + "/play"),
                Arg.Is<Dictionary<string, string>>(d => d["pinId"] == this.pinId),
                Arg.Is<Dictionary<string, string[]>>(d => d["Authorization"][0] == $"Bearer {this.accessToken}"),
                Arg.Any<object>());
        }
    }
}
