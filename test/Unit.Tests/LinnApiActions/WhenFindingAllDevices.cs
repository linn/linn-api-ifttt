namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    using FluentAssertions;

    using Linn.Api.Ifttt.Resources.Linn;
    using Linn.Common.Proxy;

    using NSubstitute;

    using Xunit;

    public class WhenFindingAllDevices : ContextBase
    {
        private readonly PlayerResource[] players;

        private readonly IDictionary<string, string> result;

        public WhenFindingAllDevices()
        {
            var accessToken = Guid.NewGuid().ToString();

            this.players = new[] { GeneratePlayerResource(), GeneratePlayerResource(), GeneratePlayerResource() };

            var response = Substitute.For<IRestResponse<PlayerResource[]>>();
            response.StatusCode.Returns(HttpStatusCode.OK);
            response.Value.Returns(this.players);

            this.RestClient.Get<PlayerResource[]>(
                    Arg.Any<CancellationToken>(),
                    Arg.Is<Uri>(uri => uri.ToString().EndsWith("http://localhost/players/")),
                    Arg.Any<Dictionary<string, string>>(),
                    Arg.Is<Dictionary<string, string[]>>(d => d["Authorization"][0] == $"Bearer {accessToken}"))
                .Returns(response);

            this.result = this.Sut.GetDeviceNames(accessToken, CancellationToken.None).Result;
        }

        [Fact]
        public void ShouldReturnAllDevices()
        {
            this.result.Should().HaveCount(this.players.Length);
            foreach (var playerResource in this.players)
            {
                this.result[playerResource.Id].Should().Be(playerResource.Name);
            }
        }
    }
}
