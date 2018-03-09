namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System.Net;
    using System.Net.Http;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources.Ifttt;

    using Xunit;

    public class WhenGettingUserInfoWithMissingAccessToken : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        public WhenGettingUserInfoWithMissingAccessToken()
        {
            this.response = this.Client.GetAsync("/ifttt/v1/user/info").Result;

            this.result = this.response.JsonBody<dynamic>();
        }

        [Fact]
        public void ShouldReturnForbidden()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public void ShouldReturnNoBody()
        {
            this.result.Should().BeNull();
        }
    }
}
