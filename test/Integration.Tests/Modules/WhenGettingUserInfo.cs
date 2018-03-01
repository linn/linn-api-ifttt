namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System;
    using System.Net;
    using System.Net.Http;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources;
    using Linn.Api.Ifttt.Service.Factories;
    using Linn.Api.Ifttt.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using Xunit;

    public class WhenGettingUserInfo
    {
        private readonly HttpResponseMessage response;

        private readonly UserInfoResource userInfoResource;

        private readonly IftttDataResource<UserInfoResource> result;

        public WhenGettingUserInfo()
        {
            var accessToken = Guid.NewGuid().ToString();

            this.userInfoResource = new UserInfoResource("A.N. Other", "/sub/myid");

            var userInfoResourceFactory = Substitute.For<IUserResourceFactory>();
            userInfoResourceFactory.Create(accessToken).Returns(this.userInfoResource);

            var client = new TestClient()
                .WithAssembly(typeof(UserInfoModule).Assembly)
                .WithAccessToken(accessToken)
                .With(s => s.AddSingleton(userInfoResourceFactory));

            this.response = client.GetAsync("/ifttt/v1/user/info").Result;

            this.result = this.response.JsonBody<IftttDataResource<UserInfoResource>>();
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

        [Fact]
        public void ShouldSetUrl()
        {
            this.result.Data.Url.Should().Be(this.userInfoResource.Url);
        }
    }
}
