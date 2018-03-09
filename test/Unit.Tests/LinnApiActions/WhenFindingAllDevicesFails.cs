namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Linn;
    using Linn.Common.Proxy;

    using NSubstitute;

    using Xunit;

    public class WhenFindingAllDevicesFails : ContextBase
    {
        private readonly Action action;

        private readonly HttpStatusCode statusCode;

        public WhenFindingAllDevicesFails()
        {
            this.statusCode = HttpStatusCode.Forbidden;

            var response = Substitute.For<IRestResponse<PlayerResource[]>>();
            response.StatusCode.Returns(this.statusCode);

            this.RestClient.Get<PlayerResource[]>(
                    Arg.Any<CancellationToken>(),
                    Arg.Any<Uri>(),
                    Arg.Any<Dictionary<string, string>>(),
                    Arg.Any<Dictionary<string, string[]>>())
                .Returns(response);

            this.action = () => this.Sut.GetDeviceNames(Guid.NewGuid().ToString(), CancellationToken.None).Wait();
        }

        [Fact]
        public void ShouldThrowLinnApiException()
        {
            this.action.Should().Throw<LinnApiException>().And.StatusCode.Should().Be(this.statusCode);
        }
    }
}
