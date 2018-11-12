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

  public class WhePlayingAnUnknownPlaylist : ContextBase
  {
    private readonly Action action;

    public WhePlayingAnUnknownPlaylist()
    {
      this.RestClient.Put(
          Arg.Any<CancellationToken>(),
          Arg.Any<Uri>(),
          Arg.Any<Dictionary<string, string>>(),
          Arg.Any<Dictionary<string, string[]>>(),
          Arg.Any<object>()).Returns(HttpStatusCode.Forbidden);

      this.action = () => this.Sut.PlayPlaylist(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), CancellationToken.None).Wait();
    }

    [Fact]
    public void ShouldThrowLinnApiException()
    {
      this.action.Should().Throw<LinnApiException>().And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
  }
}
