namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    using FluentAssertions;

    using NSubstitute;

    using Xunit;

    public class WhenUnmutingASpecificDevice : ContextBase
    {
        private readonly string result;

        private readonly string accessToken;

        private readonly string deviceId;

        public WhenUnmutingASpecificDevice()
        {
            this.accessToken = Guid.NewGuid().ToString();

            this.deviceId = Guid.NewGuid().ToString();

            this.RestClient.Delete(
                Arg.Any<CancellationToken>(),
                Arg.Any<Uri>(),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<Dictionary<string, string[]>>()).Returns(HttpStatusCode.OK);

            this.result = this.Sut.UnmuteDevice(this.accessToken, this.deviceId, CancellationToken.None).Result;
        }

        [Fact]
        public void ShouldReturnATimestamp()
        {
            DateTime.Parse(this.result).ToUniversalTime().Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void ShouldRemoveMuteOnTheDevice()
        {
            this.RestClient.Received().Delete(
                Arg.Any<CancellationToken>(),
                Arg.Is<Uri>(uri => uri.ToString() == "http://localhost/players/" + this.deviceId + "/mute"),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Is<Dictionary<string, string[]>>(d => d["Authorization"][0] == $"Bearer {this.accessToken}"));
        }
    }
}
