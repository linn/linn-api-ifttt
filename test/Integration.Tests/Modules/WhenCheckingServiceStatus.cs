namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System.Net;
    using System.Net.Http;

    using FluentAssertions;

    using Xunit;

    public class WhenCheckingServiceStatus : ContextBase
    {
        private readonly HttpResponseMessage response;

        public WhenCheckingServiceStatus()
        {
            this.response = this.Client.GetAsync("/ifttt/v1/status").Result;
        }

        [Fact]
        public void ShouldReturnOk()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
