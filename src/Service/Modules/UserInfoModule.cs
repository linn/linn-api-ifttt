namespace Linn.Api.Ifttt.Service.Modules
{
    using Botwin;
    using Botwin.Response;

    using Linn.Api.Ifttt.Resources.Ifttt;
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

                            if (userInfoResource == null)
                            {
                                res.StatusCode = 401;
                            }
                            else
                            {
                                var resource = new DataResource<UserInfoResource>(userInfoResource);
                                await res.AsJson(resource);
                            }
                        }
                    });
        }
    }
}
