namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System.Net;
    using System.Net.Http;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources.Ifttt;

    using Newtonsoft.Json;

    using Xunit;

    public class WhenInvokePinOnASpecificDeviceWithMissingPlaylistIdorDeviceId : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        public WhenInvokePinOnASpecificDeviceWithMissingPlaylistIdorDeviceId()
        {
            var request = new
                              {
                                  actionFields = new { },
                                  ifttt_source = new { id = "2", url = "https://ifttt.com/myrecipes/personal/2" },
                                  user = new { timezone = "Pacific Time (US & Canada)" }
                              };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.Client.SetAccessToken(this.AccessToken);

            this.response = this.Client.PostAsync("/ifttt/v1/actions/invoke_pin", content).Result;

            this.result = this.response.JsonBody<ErrorResource>();
        }

        [Fact]
        public void ShouldReturnBadRequest()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void ShouldDescribeError()
        {
            this.result.Errors.Should().HaveCount(2);
            this.result.Errors.Should().Contain(e => e.Message == "Action field `pin_id` missing");
            this.result.Errors.Should().Contain(e => e.Message == "Action field `device_id` missing");
        }
    }
}
