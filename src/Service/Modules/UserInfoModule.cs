namespace Linn.Api.Ifttt.Service.Modules
{
    using Botwin;
    using Botwin.Response;

    using Linn.Api.Ifttt.Resources;
    using Linn.Api.Ifttt.Service.Factories;

    public class UserInfoModule : BotwinModule
    {
        public UserInfoModule(IUserResourceFactory userResourceFactory)
        {
            this.Get(
                "/ifttt/v1/user/info",
                async (req, res, routeData) =>
                    {
                        var accessToken = req.GetAccessToken();

                        if (accessToken == null)
                        {
                            res.StatusCode = 401;
                        }
                        else
                        {
                            var userInfoResource = await userResourceFactory.Create(accessToken);
                            var resource = new IftttDataResource<UserInfoResource>(userInfoResource);
                            await res.AsJson(resource);
                        }
                    });
        }
    }
}
