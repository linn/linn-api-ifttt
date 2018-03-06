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

    using Xunit;

    public class WhenPlayMediaOnASpecificDevice : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly DataResource<ActionResponse[]> result;

        public WhenPlayMediaOnASpecificDevice()
        {
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

            this.LinnApiActions.PlaySingleMedia(this.AccessToken, deviceId, mediaUrl, mediaTitle, mediaArtworkUrl, Arg.Any<CancellationToken>()).Returns("id");

            this.Client.SetAccessToken(this.AccessToken);

            this.response = this.Client.PostAsync("/ifttt/v1/actions/play_single_media", content).Result;

            this.result = this.response.JsonBody<DataResource<ActionResponse[]>>();
        }

        [Fact]
        public void ShouldReturnOk()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public void ShouldReturnSomethingIntheId()
        {
            this.result.Data[0].Id.Should().NotBeNullOrEmpty();
        }
    }
}
