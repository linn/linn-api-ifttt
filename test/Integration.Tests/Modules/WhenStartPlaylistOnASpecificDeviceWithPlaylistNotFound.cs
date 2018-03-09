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

    public class WhenStartPlaylistOnASpecificDeviceWithPlaylistNotFound : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        public WhenStartPlaylistOnASpecificDeviceWithPlaylistNotFound()
        {
            var deviceId = Guid.NewGuid().ToString();

            var playlistId = Guid.NewGuid().ToString();

            var request = new
                              {
                                  actionFields = new { device_id = deviceId, playlist_id = playlistId },
                                  ifttt_source = new { id = "2", url = "https://ifttt.com/myrecipes/personal/2" },
                                  user = new { timezone = "Pacific Time (US & Canada)" }
                              };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.LinnApiActions.PlayPlaylist(Arg.Any<string>(), deviceId, playlistId, Arg.Any<CancellationToken>())
                .Throws(new LinnApiException(HttpStatusCode.NotFound));

            this.Client.SetAccessToken(Guid.NewGuid().ToString());

            this.response = this.Client.PostAsync("/ifttt/v1/actions/play_playlist", content).Result;

            this.result = this.response.JsonBody<ErrorResource>();
        }

        [Fact]
        public void ShouldReturnBadRequest()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void ShouldReturnSkipMessage()
        {
            this.result.Errors.Should().HaveCount(1);
            this.result.Errors[0].Status.Should().Be("SKIP");
            this.result.Errors[0].Message.Should().Be("Linn API status code: NotFound");
        }
    }
}
