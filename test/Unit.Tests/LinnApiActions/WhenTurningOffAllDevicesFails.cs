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

    public class WhenTurningOffAllDevicesFails : ContextBase
    {
        private readonly HttpStatusCode statusCode;

        private readonly Action action;

        public WhenTurningOffAllDevicesFails()
        {
            this.statusCode = HttpStatusCode.Forbidden;

            var accessToken = Guid.NewGuid().ToString();

            var players = new[] { GeneratePlayerResource(), GeneratePlayerResource(), GeneratePlayerResource() };

            var response = Substitute.For<IRestResponse<PlayerResource[]>>();
            response.StatusCode.Returns(HttpStatusCode.OK);
            response.Value.Returns(players);

            this.RestClient.Get<PlayerResource[]>(
                    Arg.Any<CancellationToken>(),
                    Arg.Is<Uri>(uri => uri.ToString().EndsWith("http://localhost/players/")),
                    Arg.Any<Dictionary<string, string>>(),
                    Arg.Is<Dictionary<string, string[]>>(d => d["Authorization"][0] == $"Bearer {accessToken}"))
                .Returns(response);

            this.RestClient.Put(
                Arg.Any<CancellationToken>(),
                Arg.Any<Uri>(),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<Dictionary<string, string[]>>(),
                Arg.Any<object>()).Returns(this.statusCode);

            this.action = () => this.Sut.TurnOffAllDevices(accessToken, CancellationToken.None).Wait();
        }

        [Fact]
        public void ShouldThrowLinnApiException()
        {
            this.action.Should().Throw<LinnApiException>().And.StatusCode.Should().Be(this.statusCode);
        }
    }
}
