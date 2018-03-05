namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System.Net;
    using System.Net.Http;

    using FluentAssertions;

    using Xunit;

    public class WhenCheckingServiceStatusWithInvalidServiceKey : ContextBase
    {
        private readonly HttpResponseMessage response;

        public WhenCheckingServiceStatusWithInvalidServiceKey()
        {
            this.Client.DefaultRequestHeaders.Add("IFTTT-Service-Key", new[] { "INVALID" });
            this.response = this.Client.GetAsync("/ifttt/v1/status").Result;
        }

        [Fact]
        public void ShouldReturnUnauthorised()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
