namespace Linn.Api.Ifttt.Tests
{
    using System.IO;

    using Amazon.Lambda.APIGatewayEvents;
    using Amazon.Lambda.TestUtilities;

    using FluentAssertions;

    using Newtonsoft.Json;

    using Resources;

    using Xunit;

    public class UserInfoControllerTests
    {
        private readonly IftttDataResource<UserInfoResource> result;

        private readonly APIGatewayProxyResponse response;

        public UserInfoControllerTests()
        {
            var lambdaFunction = new LambdaEntryPoint();

            var requestStr = File.ReadAllText("./SampleRequests/UserInfoController-Get.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            var context = new TestLambdaContext();

            this.response = lambdaFunction.FunctionHandlerAsync(request, context).Result;
            this.result = JsonConvert.DeserializeObject<IftttDataResource<UserInfoResource>>(this.response.Body);
        }

        [Fact]
        public void ShouldHaveCorrectStatusCode()
        {
            this.response.StatusCode.Should().Be(200);
        }

        [Fact]
        public void ShouldHaveCorrectContentType()
        {
            this.response.Headers.Should().ContainKey("Content-Type").And.Subject["Content-Type"].Should().Be("application/json; charset=utf-8");
        }

        [Fact]
        public void ShouldContainName()
        {
            this.result.Data.Name.Should().Be("A.N. User");
        }

        [Fact]
        public void ShouldContainId()
        {
            this.result.Data.Id.Should().Be("/sub/userid");
        }

        [Fact]
        public void ShouldNotContainAUrl()
        {
            this.result.Data.Url.Should().BeNullOrEmpty();
        }
    }
}
