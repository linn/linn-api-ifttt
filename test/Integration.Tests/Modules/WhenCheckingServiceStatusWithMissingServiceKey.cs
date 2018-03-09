namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System.Net;
    using System.Net.Http;

    using FluentAssertions;

    using Xunit;

    public class WhenCheckingServiceStatusWithMissingServiceKey : ContextBase
    {
        private readonly HttpResponseMessage response;

        public WhenCheckingServiceStatusWithMissingServiceKey()
        {
            this.response = this.Client.GetAsync("/ifttt/v1/status").Result;
        }

        [Fact]
        public void ShouldReturnUnauthorised()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
