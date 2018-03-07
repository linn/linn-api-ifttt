namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources.Ifttt;

    using NSubstitute;

    using Xunit;

    public class WhenGettingUserInfo : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly UserInfoResource userInfoResource;

        private readonly DataResource<UserInfoResource> result;

        public WhenGettingUserInfo()
        {
            this.userInfoResource = new UserInfoResource("A.N. Other", "/sub/myid");

            this.UserInfoResourceFactory.Create(this.AccessToken, Arg.Any<CancellationToken>()).Returns(this.userInfoResource);

            this.Client.SetAccessToken(this.AccessToken);

            this.response = this.Client.GetAsync("/ifttt/v1/user/info").Result;

            this.result = this.response.JsonBody<DataResource<UserInfoResource>>();
        }

        [Fact]
        public void ShouldReturnOk()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public void ShouldSetName()
        {
            this.result.Data.Name.Should().Be(this.userInfoResource.Name);
        }

        [Fact]
        public void ShouldSetId()
        {
            this.result.Data.Id.Should().Be(this.userInfoResource.Id);
        }
    }
}
