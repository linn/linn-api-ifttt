namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources.Ifttt;

    using Newtonsoft.Json;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using Xunit;

    public class WhenPlayMediaOnASpecificDeviceWithInvalidAccessToken : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        private readonly string errorMessage;

        public WhenPlayMediaOnASpecificDeviceWithInvalidAccessToken()
        {
            this.errorMessage = "Failure";

            var deviceId = Guid.NewGuid().ToString();

            var mediaUrl = "https://localhost/" + Guid.NewGuid();

            var mediaTitle = "Title";

            var mediaArtworkUrl = mediaUrl + "/artwork.jpg";

            var request = new
                              {
                                  actionFields = new { device_id = deviceId, media_url = mediaUrl, media_title = mediaTitle, media_artwork_url = mediaArtworkUrl },
                                  ifttt_source = new { id = "2", url = "https://ifttt.com/myrecipes/personal/2" },
                                  user = new { timezone = "Pacific Time (US & Canada)" }
                              };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.LinnApiActions.PlaySingleMedia(Arg.Any<string>(), deviceId, mediaUrl, mediaTitle, mediaArtworkUrl, Arg.Any<CancellationToken>()).Throws(new Exception(this.errorMessage));

            this.Client.SetAccessToken(Guid.NewGuid().ToString());

            this.response = this.Client.PostAsync("/ifttt/v1/actions/play_single_media", content).Result;

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
            this.result.Errors[0].Message.Should().Be(this.errorMessage);
        }
    }
}
