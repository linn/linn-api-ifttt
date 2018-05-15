namespace Linn.Api.Ifttt.Service.Factories
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using IdentityModel.Client;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Ifttt;
    using Linn.Common.Configuration;

    public class UserResourceFactory : IUserResourceFactory
    {
        private readonly Task<DiscoveryResponse> discoveryResponseTask;

        public UserResourceFactory()
        {
            this.discoveryResponseTask = DiscoveryClient.GetAsync(ConfigurationManager.Configuration["discoveryEndpoint"]);
        }

        public async Task<UserInfoResource> Create(string accessToken, CancellationToken ct)
        {
            var doc = await this.discoveryResponseTask;

            if (doc.StatusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(doc.StatusCode);
            }

            var userInfoClient = new UserInfoClient(doc.UserInfoEndpoint);

            var userInfoResponse = await userInfoClient.GetAsync(accessToken, ct);

            if (userInfoResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new LinnApiException(userInfoResponse.HttpStatusCode);
            }

            var email = userInfoResponse.Claims.First(c => c.Type == "email").Value;
            var accountId = userInfoResponse.Claims.First(c => c.Type == "sub").Value;

            return new UserInfoResource(email, accountId);
        }
    }
}