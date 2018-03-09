namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources.Linn;
    using Linn.Common.Proxy;

    using NSubstitute;

    using Xunit;

    public class WhenTurningOffAllDevices : ContextBase
    {
        private readonly string result;

        private readonly PlayerResource[] players;

        private readonly string accessToken;

        public WhenTurningOffAllDevices()
        {
            this.accessToken = Guid.NewGuid().ToString();

            this.players = new[] { GeneratePlayerResource(), GeneratePlayerResource(), GeneratePlayerResource() };

            var response = Substitute.For<IRestResponse<PlayerResource[]>>();
            response.StatusCode.Returns(HttpStatusCode.OK);
            response.Value.Returns(this.players);

            this.RestClient.Get<PlayerResource[]>(
                    Arg.Any<CancellationToken>(),
                    Arg.Is<Uri>(uri => uri.ToString().EndsWith("http://localhost/players/")),
                    Arg.Any<Dictionary<string, string>>(),
                    Arg.Is<Dictionary<string, string[]>>(d => d["Authorization"][0] == $"Bearer {this.accessToken}"))
                .Returns(response);

            this.RestClient.Put(
                Arg.Any<CancellationToken>(),
                Arg.Any<Uri>(),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<Dictionary<string, string[]>>(),
                Arg.Any<object>()).Returns(HttpStatusCode.OK);

            this.result = this.Sut.TurnOffAllDevices(this.accessToken, CancellationToken.None).Result;
        }

        [Fact]
        public void ShouldReturnATimestamp()
        {
            DateTime.Parse(this.result).Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void ShouldSetStandbyOnAllDevices()
        {
            var standbyLinks = this.players.Select(p => p.Links.First(l => l.Rel == "standby").Href);
            foreach (var href in standbyLinks)
            {
                this.RestClient.Received().Put(
                    Arg.Any<CancellationToken>(),
                    Arg.Is<Uri>(uri => uri.ToString() == "http://localhost" + href),
                    Arg.Any<Dictionary<string, string>>(),
                    Arg.Is<Dictionary<string, string[]>>(d => d["Authorization"][0] == $"Bearer {this.accessToken}"),
                    Arg.Any<object>());
            }
        }
    }
}
