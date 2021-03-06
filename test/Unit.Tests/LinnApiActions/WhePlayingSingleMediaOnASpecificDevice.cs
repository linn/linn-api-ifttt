﻿namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Proxies.Models;

    using NSubstitute;

    using Xunit;

    public class WhePlayingSingleMediaOnASpecificDevice : ContextBase
    {
        private readonly string result;

        private readonly string accessToken;

        private readonly string deviceId;

        private readonly string mediaUrl;

        private readonly string mediaTitle;

        private readonly string mediaArtworkUrl;

        private PlayableItemModel payload;

        public WhePlayingSingleMediaOnASpecificDevice()
        {
            this.accessToken = Guid.NewGuid().ToString();

            this.deviceId = Guid.NewGuid().ToString();

            this.mediaTitle = "MY_TITLE";

            this.mediaArtworkUrl = "http://localhost/media/linn.jpg";

            this.mediaUrl = "http://localhost/media/linn.flac";

            this.RestClient.Put(
                Arg.Any<CancellationToken>(),
                Arg.Any<Uri>(),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<Dictionary<string, string[]>>(),
                Arg.Do<PlayableItemModel>(d => this.payload = d)).Returns(HttpStatusCode.OK);

            this.result = this.Sut.PlaySingleMedia(this.accessToken, this.deviceId, this.mediaUrl, this.mediaTitle, this.mediaArtworkUrl, CancellationToken.None).Result;
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
                Arg.Any<Dictionary<string, string>>(),
                Arg.Is<Dictionary<string, string[]>>(d => d["Authorization"][0] == $"Bearer {this.accessToken}"),
                Arg.Any<PlayableItemModel>());
            this.payload.Uri.Should().Be(this.mediaUrl);
            this.payload.Title.Should().Be(this.mediaTitle);
            this.payload.Artwork.Should().Be(this.mediaArtworkUrl);
        }
    }
}
