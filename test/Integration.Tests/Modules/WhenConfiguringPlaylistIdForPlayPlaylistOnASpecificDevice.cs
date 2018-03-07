namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources.Ifttt;

    using Newtonsoft.Json;

    using NSubstitute;

    using Xunit;

    public class WhenConfiguringPlaylistIdForPlayPlaylistOnASpecificDevice : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly DataResource<ActionFieldOption[]> result;

        private readonly Dictionary<string, string> playlists;

        public WhenConfiguringPlaylistIdForPlayPlaylistOnASpecificDevice()
        {
            var request = new { };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.playlists = new Dictionary<string, string>
                              {
                                  [Guid.NewGuid().ToString()] = "Kids Bathtime",
                                  [Guid.NewGuid().ToString()] = "Holiday Drive",
                                  [Guid.NewGuid().ToString()] = "Poker Night",
                                  [Guid.NewGuid().ToString()] = "Morning After Blues"
            };

            this.LinnApiActions.GetPlaylistNames(this.AccessToken, Arg.Any<CancellationToken>()).Returns(this.playlists);

            this.Client.SetAccessToken(this.AccessToken);

            this.response = this.Client.PostAsync("/ifttt/v1/actions/play_playlist/fields/playlist_id/options", content).Result;

            this.result = this.response.JsonBody<DataResource<ActionFieldOption[]>>();
        }

        [Fact]
        public void ShouldReturnOk()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public void ShouldReturnData()
        {
            this.result.Data.Should().HaveCount(this.playlists.Count);
            foreach (var device in this.playlists)
            {
                this.result.Data.First(d => d.Value == device.Key).Label.Should().Be(device.Value);
            }
        }
    }
}
