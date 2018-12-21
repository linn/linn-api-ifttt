using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    public class WhenSelectingASourceOnASpecificDevice : ContextBase
    {
        private readonly string result;

        private readonly string accessToken;

        private readonly string deviceId;

        private readonly string sourceId;

        public WhenSelectingASourceOnASpecificDevice()
        {
            this.accessToken = Guid.NewGuid().ToString();

            this.deviceId = Guid.NewGuid().ToString();

            this.sourceId = Guid.NewGuid().ToString();

            this.RestClient.Put(
                Arg.Any<CancellationToken>(),
                Arg.Any<Uri>(),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<Dictionary<string, string[]>>(),
                Arg.Any<object>()).Returns(HttpStatusCode.OK);

            this.result = this.Sut.SelectSource(this.accessToken, this.deviceId, this.sourceId, CancellationToken.None).Result;
        }

        [Fact]
        public void ShouldReturnATimestamp()
        {
            DateTime.Parse(this.result).ToUniversalTime().Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void ShouldSelectSourceOnTheDevice()
        {
            this.RestClient.Received().Put(
                Arg.Any<CancellationToken>(),
                Arg.Is<Uri>(uri => uri.ToString() == $"http://localhost/players/{this.deviceId}/source"),
                Arg.Is<Dictionary<string, string>>(p => p["sourceId"] == this.sourceId),
                Arg.Is<Dictionary<string, string[]>>(d => d["Authorization"][0] == $"Bearer {this.accessToken}"),
                Arg.Any<object>());
        }
    }
}
