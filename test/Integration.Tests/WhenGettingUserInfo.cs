namespace Linn.Api.Ifttt.Testing.Integration
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;

    using Botwin;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources;
    using Linn.Api.Ifttt.Service.Factories;
    using Linn.Api.Ifttt.Service.Modules;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    using NSubstitute;

    using Xunit;

    public class WhenGettingUserInfo
    {
        private readonly HttpResponseMessage response;

        private readonly UserInfoResource userInfoResource;

        private IftttDataResource<UserInfoResource> result;

        public WhenGettingUserInfo()
        {
            var accessToken = Guid.NewGuid().ToString();

            this.userInfoResource = new UserInfoResource("A.N. Other", "/sub/myid");

            var userInfoResourceFactory = Substitute.For<IUserResourceFactory>();
            userInfoResourceFactory.Create(accessToken).Returns(this.userInfoResource);

            var server = new TestServer(
                WebHost.CreateDefaultBuilder()
                .ConfigureServices(
                    services =>
                        {
                            services.AddBotwin(typeof(UserInfoModule).Assembly);
                            services.AddSingleton(userInfoResourceFactory);
                        })
                        .Configure(app => app.UseBotwin()));

            var client = server.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            this.response = client.GetAsync("/ifttt/v1/user/info").Result;

            this.result =
                JsonConvert.DeserializeObject<IftttDataResource<UserInfoResource>>(
                    this.response.Content.ReadAsStringAsync().Result);
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
