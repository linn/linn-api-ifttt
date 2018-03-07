namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    using FluentAssertions;

    using NSubstitute;

    using Xunit;

    public class WhePlayingAPlaylistOnASpecificDevice : ContextBase
    {
        private readonly string result;

        private readonly string accessToken;

        private readonly string deviceId;

        private readonly string playlistId;

        public WhePlayingAPlaylistOnASpecificDevice()
        {
            this.accessToken = Guid.NewGuid().ToString();

            this.deviceId = Guid.NewGuid().ToString();

            this.playlistId = Guid.NewGuid().ToString();

            this.RestClient.Put(
                Arg.Any<CancellationToken>(),
                Arg.Any<Uri>(),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<Dictionary<string, string[]>>(),
                Arg.Any<object>()).Returns(HttpStatusCode.OK);

            this.result = this.Sut.PlayPlaylist(this.accessToken, this.deviceId, this.playlistId, CancellationToken.None).Result;
        }

        [Fact]
        public void ShouldReturnATimestamp()
        {
            DateTime.Parse(this.result).Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void ShouldPlayOnTheDevice()
        {
            this.RestClient.Received().Put(
                Arg.Any<CancellationToken>(),
                Arg.Is<Uri>(uri => uri.ToString() == "http://localhost/players/" + this.deviceId + "/playlist/"),
                Arg.Is<Dictionary<string, string>>(p => p["playlistId"] == this.playlistId),
                Arg.Is<Dictionary<string, string[]>>(d => d["Authorization"][0] == $"Bearer {this.accessToken}"),
                Arg.Any<object>());
        }
    }
}
