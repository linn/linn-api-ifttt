namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Proxies;

    using NSubstitute;

    using Xunit;

    public class WhenUnmutingASpecificDeviceFails : ContextBase
    {
        private readonly HttpStatusCode statusCode;

        private readonly Action action;

        public WhenUnmutingASpecificDeviceFails()
        {
            this.statusCode = HttpStatusCode.Forbidden;

            this.RestClient.Delete(
                Arg.Any<CancellationToken>(),
                Arg.Any<Uri>(),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<Dictionary<string, string[]>>()).Returns(this.statusCode);

            this.action = () => this.Sut.UnmuteDevice(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), CancellationToken.None).Wait();
        }

        [Fact]
        public void ShouldThrowLinnApiException()
        {
            this.action.Should().Throw<LinnApiException>().And.StatusCode.Should().Be(this.statusCode);
        }
    }
}
