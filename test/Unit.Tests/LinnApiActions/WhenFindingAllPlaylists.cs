namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources.Linn;
    using Linn.Common.Proxy;

    using NSubstitute;

    using Xunit;

    public class WhenFindingAllPlaylists : ContextBase
    {
        private readonly PlaylistResource[] playlists;

        private readonly IDictionary<string, string> result;

        public WhenFindingAllPlaylists()
        {
            var accessToken = Guid.NewGuid().ToString();

            this.playlists = new[] { GeneratePlaylistResource(), GeneratePlaylistResource(), GeneratePlaylistResource() };

            var response = Substitute.For<IRestResponse<PlaylistResource[]>>();
            response.StatusCode.Returns(HttpStatusCode.OK);
            response.Value.Returns(this.playlists);

            this.RestClient.Get<PlaylistResource[]>(
                    Arg.Any<CancellationToken>(),
                    Arg.Is<Uri>(uri => uri.ToString().EndsWith("http://localhost/playlists/")),
                    Arg.Is<Dictionary<string, string>>(d => d["sortBy"] == "name" && d["includeOnly"] == "named"),
                    Arg.Is<Dictionary<string, string[]>>(d => d["Authorization"][0] == $"Bearer {accessToken}"))
                .Returns(response);

            this.result = this.Sut.GetPlaylistNames(accessToken, CancellationToken.None).Result;
        }

        [Fact]
        public void ShouldReturnAllPlaylists()
        {
            this.result.Should().HaveCount(this.playlists.Length);
            foreach (var playlistResource in this.playlists)
            {
                this.result[playlistResource.Id].Should().Be(playlistResource.Name);
            }
        }
    }
}
