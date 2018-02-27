namespace Linn.Api.Ifttt.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using IdentityModel.Client;

    using Linn.Api.Ifttt.Resources;

    using Microsoft.Extensions.Configuration;

    public class UserResourceFactory : IUserResourceFactory
    {
        private readonly Task<DiscoveryResponse> discoveryResponseTask;

        public UserResourceFactory(IConfiguration configuration)
        {
            var discoveryClient = new DiscoveryClient(configuration["discoveryEndpoint"]);
            this.discoveryResponseTask = discoveryClient.GetAsync();
        }

        public async Task<UserInfoResource> Create(string accessToken)
        {
            var doc = await this.discoveryResponseTask;
            var userInfoClient = new UserInfoClient(doc.UserInfoEndpoint);

            var userInfoResponse = await userInfoClient.GetAsync(accessToken);

            var email = userInfoResponse.Claims.First(c => c.Type == "email").Value;
            var accountId = userInfoResponse.Claims.First(c => c.Type == "sub").Value;

            return new UserInfoResource(email, accountId);
        }
    }
}