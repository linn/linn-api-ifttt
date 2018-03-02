namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System;
    using System.Net;
    using System.Net.Http;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources.Ifttt;

    using NSubstitute;

    using Xunit;

    public class WhenGettingUserInfoWithInvalidAccessToken : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly DataResource<UserInfoResource> result;

        public WhenGettingUserInfoWithInvalidAccessToken()
        {
            var userInfoResource = new UserInfoResource("A.N. Other", "/sub/myid");
            this.UserInfoResourceFactory.Create(this.AccessToken).Returns(userInfoResource);

            this.Client.SetAccessToken(Guid.NewGuid().ToString());

            this.response = this.Client.GetAsync("/ifttt/v1/user/info").Result;

            this.result = this.response.JsonBody<DataResource<UserInfoResource>>();
        }

        [Fact]
        public void ShouldReturnForbidden()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public void ShouldNotReturnBody()
        {
            this.result.Should().BeNull();
        }
    }
}
