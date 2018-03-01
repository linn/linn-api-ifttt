namespace Linn.Api.Ifttt.Service.Factories
{
    using System.Linq;
    using System.Threading.Tasks;

    using IdentityModel.Client;

    using Linn.Api.Ifttt.Resources;

    public class UserResourceFactory : IUserResourceFactory
    {
        private readonly Task<DiscoveryResponse> discoveryResponseTask;

        public UserResourceFactory(string discoveryEndpoint)
        {
            var discoveryClient = new DiscoveryClient(discoveryEndpoint);
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