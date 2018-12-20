using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

using FluentAssertions;

using Linn.Api.Ifttt.Resources.Linn;
using Linn.Common.Proxy;

using NSubstitute;

using Xunit;

namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    public class WhenFindingAllDeviceSources : ContextBase
    {
        private readonly PlayerResource player;

        private readonly IDictionary<string, string> result;

        public WhenFindingAllDeviceSources()
        {
            var accessToken = Guid.NewGuid().ToString();

            this.player =  GeneratePlayerResource();

            var response = Substitute.For<IRestResponse<PlayerResource>>();
            response.StatusCode.Returns(HttpStatusCode.OK);
            response.Value.Returns(this.player);

            this.RestClient.Get<PlayerResource>(
                    Arg.Any<CancellationToken>(),
                    Arg.Is<Uri>(uri => uri.ToString() == $"http://localhost/players/{this.player.Id}"),
                    null,
                    Arg.Is<Dictionary<string, string[]>>(d => d["Authorization"][0] == $"Bearer {accessToken}"))
                .Returns(response);

            this.result = this.Sut.GetDeviceSourceNames(accessToken, this.player.Id, CancellationToken.None).Result;
        }

        [Fact]
        public void ShouldReturnAllDeviceSources()
        {
            this.result.Should().HaveCount(this.player.Sources.Length);
            foreach (var sourceResource in this.player.Sources)
            {
                this.result[sourceResource.Id].Should().Be(sourceResource.Name);
            }
        }
    }
}
