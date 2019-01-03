using System;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Linn.Api.Ifttt.Resources.Ifttt;
using Newtonsoft.Json;
using Xunit;

namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    public class WhenSelectSourceOnASpecificDeviceWithMissingSourceIdorDeviceId : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        public WhenSelectSourceOnASpecificDeviceWithMissingSourceIdorDeviceId()
        {
            var request = new
            {
                actionFields = new { },
                ifttt_source = new { id = "2", url = "https://ifttt.com/myrecipes/personal/2" },
                user = new { timezone = "Pacific Time (US & Canada)" }
            };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.Client.SetAccessToken(this.AccessToken);

            this.response = this.Client.PostAsync("/ifttt/v1/actions/select_source", content).Result;

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
            this.result.Errors.Should().Contain(e => e.Message == "Action field `source_id` missing");
            this.result.Errors.Should().Contain(e => e.Message == "Action field `device_id` missing");
        }
    }
}
