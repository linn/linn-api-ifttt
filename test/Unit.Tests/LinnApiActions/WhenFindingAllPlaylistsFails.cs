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

    public class WhenFindingAllPlaylistsFails : ContextBase
    {
        private readonly HttpStatusCode statusCode;

        private readonly Action action;

        public WhenFindingAllPlaylistsFails()
        {
            this.statusCode = HttpStatusCode.Forbidden;

            var response = Substitute.For<IRestResponse<PlaylistResource[]>>();
            response.StatusCode.Returns(this.statusCode);

            this.RestClient.Get<PlaylistResource[]>(
                    Arg.Any<CancellationToken>(),
                    Arg.Any<Uri>(),
                    Arg.Any<Dictionary<string, string>>(),
                    Arg.Any<Dictionary<string, string[]>>())
                .Returns(response);

            this.action = () => this.Sut.GetPlaylistNames(Guid.NewGuid().ToString(), CancellationToken.None).Wait();
        }

        [Fact]
        public void ShouldThrowLinnApiException()
        {
            this.action.Should().Throw<LinnApiException>().And.StatusCode.Should().Be(this.statusCode);
        }
    }
}
