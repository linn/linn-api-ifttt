namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Ifttt;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using Xunit;

    public class WhenGettingUserInfoWithInvalidAccessToken : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        public WhenGettingUserInfoWithInvalidAccessToken()
        {
            this.UserInfoResourceFactory.Create(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Throws(new LinnApiException(HttpStatusCode.Forbidden));

            this.Client.SetAccessToken(Guid.NewGuid().ToString());

            this.response = this.Client.GetAsync("/ifttt/v1/user/info").Result;

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
            this.result.Errors[0].Message.Should().Be("Linn API status code: Forbidden");
        }
    }
}
