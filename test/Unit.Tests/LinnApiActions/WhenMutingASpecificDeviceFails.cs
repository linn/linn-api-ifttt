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

    public class WhenMutingASpecificDeviceFails : ContextBase
    {
        private readonly HttpStatusCode statusCode;

        private readonly Action action;

        public WhenMutingASpecificDeviceFails()
        {
            this.statusCode = HttpStatusCode.Forbidden;

            this.RestClient.Put(
                Arg.Any<CancellationToken>(),
                Arg.Any<Uri>(),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<Dictionary<string, string[]>>(),
                Arg.Any<object>()).Returns(this.statusCode);

            this.action = () => this.Sut.MuteDevice(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), CancellationToken.None).Wait();
        }

        [Fact]
        public void ShouldThrowLinnApiException()
        {
            this.action.Should().Throw<LinnApiException>().And.StatusCode.Should().Be(this.statusCode);
        }
    }
}
