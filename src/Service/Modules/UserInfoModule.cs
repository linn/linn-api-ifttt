namespace Linn.Api.Ifttt.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Api.Ifttt.Resources.Ifttt;
    using Linn.Api.Ifttt.Service.Factories;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class UserInfoModule : CarterModule
    {
        private readonly IUserResourceFactory userResourceFactory;

        public UserInfoModule(IUserResourceFactory userResourceFactory)
        {
            this.userResourceFactory = userResourceFactory;

            this.RequiresAccessToken();

            this.Get("/ifttt/v1/user/info", this.Handler);
        }

        private async Task Handler(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            var userInfoResource = await this.userResourceFactory.Create(req.GetAccessToken(), req.HttpContext.RequestAborted);

            await res.AsJson(new DataResource<UserInfoResource>(userInfoResource), req.HttpContext.RequestAborted);
        }
    }
}
